﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication6
{
    interface UnitLife
    {
        void destroy();
        void lvlup();
    }

    class Unit : UnitLife  //описание персонажа
    {
        public int defhp;
        public int defmp;
        public double defspeed;
        private int hp;
        public int Hp
        {
            get
            {
                return hp;
            }
            set
            {
                if (hp > 100) hp = 100;
                if ((hp < 100) && (hp > 0)) Regen(hpregen);
                if (hp <= 0) destroy();
                hp = value;
            }
        }
        public double speed;

        public int damage;
        private int hpregen;
        public int mp;
        public int exp;
        private int lvl;
        public skill[] Skills = new skill[3];
        private int rename;

        public Unit(int hp, double speed, int damage, int hpregen, int mp, params skill[] Skills)
        {
            this.hp = hp;
            this.speed = speed;

            this.damage = damage;
            this.hpregen = hpregen;
            this.mp = mp;
            exp = 0;
            lvl = 0;
            this.Skills = Skills;
            defhp = hp;
            defmp = mp;
            defspeed = speed;
        }        

        public void destroy()
        {
            //Событие удаления персонажа со сцены, анимация смерти.
        }
        public void UpDATE(object source)
        {           
                hp += hpregen;                                 
        }
        public void Regen(double hpregen)
        {
            while (hp < 100)
            {
                TimerCallback timeCB = new TimerCallback(UpDATE);
                Timer time = new Timer(timeCB, null, 0, 100);
            }
        }

        public void lvlup()
        {
            if (exp == 100)
            {
                lvl++;
                exp = 0;
            }
        }

         public void stop(skill skill1)
        {
            skill1.cd = true;
        }
        public void refresh(object state)
        {
           Skills[rename].cd = false;
        }
        public void action(skill skill1, Unit target)
        {
            if (skill1.cd == false)
            {
                
                TimerCallback timeCB = new TimerCallback(refresh);
                Timer time = new Timer(timeCB, null, 0, (int)skill1.reloading * 100);
             
                if (mp >= skill1.manacost)
                {
                    mp -= skill1.manacost;
                    skill1.action(target);
                    stop(skill1);
                    rename = Array.IndexOf(Skills, skill1);
                }                
                else
                {
                    Console.WriteLine("Не хватает маны");
                }
            }
            else
            {
                Console.WriteLine("Перезарядка!");
            }
        }        
    }
    class skill
    {
        Unit targetcgange;
        public  bool cd;
        public bool buffskill;
        public int mpchange;
        public double speed;
        public int damage;
        public int manacost;        
        public double reloading;
        public double timed;

        public skill (int damage, int mpchange, double speed, int manacost, double reloading, double timed, bool buffskill)
        {
            this.damage = damage;
            this.mpchange = mpchange;
            this.speed = speed;
            this.manacost = manacost;
            this.reloading = reloading;
            this.timed = timed;
            this.buffskill = buffskill;
            cd = false;
        }
        public void def(object stat)
            {
            targetcgange.Hp = targetcgange.defhp;
            targetcgange.mp = targetcgange.defmp;
            targetcgange.speed = targetcgange.defspeed;
        }

      public void action(Unit target)
        {
            TimerCallback timeCB = new TimerCallback(def);
            Timer time = new Timer(timeCB, null, 0, (int)timed * 100);
            targetcgange = target;
            if (buffskill == true)
                {
                    target.Hp += damage;
                    target.speed += speed;
                    target.mp += mpchange;
                }
                else
                {
                    target.Hp -= damage;
                    target.speed -= speed;
                    target.mp -= mpchange;
                }           
               
        }
        
    }  
    class Program
    {

        public static void Main()
        {
            

            skill FireBall = new skill(30, 0, 0, 60, 4.00, 2.00, false);
            skill heal = new skill(20, 0, 20, 40, 4.00, 2.00, true);
            skill debuff = new skill(40, 0, 30, 50, 4.00, 2.00, false);

            Unit Eric = new Unit(100, 100.00, 100, 4, 100, FireBall, heal, debuff);
            Unit Bob = new Unit(100, 100.00, 100, 4, 100, FireBall, heal, debuff);

            Console.WriteLine("hp " + Eric.Hp + " mp " + Eric.mp + " speed " + Eric.speed + "\n");

            Eric.action(Eric.Skills[0], Eric);
            
            Console.WriteLine("hp: " + Eric.Hp);
            Console.WriteLine("mp: " + Eric.mp);
            Console.WriteLine("speed: " + Eric.speed);
            Eric.action(Eric.Skills[2], Eric);

            Console.WriteLine("hp: " + Eric.Hp);
            Console.WriteLine("mp: " + Eric.mp);
            Console.WriteLine("speed: " + Eric.speed);
            Console.ReadLine();
        } 
    }
    
}
