using Asteroids.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asteroids
{
    public class Game : BaseScene
    {
        private List<BaseObject> asteroids;
        private List<BaseObject> stars;
        private Rocket rocket;
        private List<Bullet> bullets;
        private Random random = new Random();
        private Timer timer;
        private int startAsteroidCount = 6;
        public int asteroidCounter = 0;

        public event Action<string> LogAction;

        
        public override void Init(Form form)
        {
            base.Init(form);
            
            Load();

            timer = new Timer { Interval = 60 };
            timer.Start();
            timer.Tick += Timer_Tick;
            rocket.DieEvent += Finish;
        }

        public override void SceneKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) rocket.MoveY(1);
            if (e.KeyCode == Keys.Down) rocket.MoveY(-1);
            if (e.KeyCode == Keys.ControlKey)
            {
                if (bullets.Count <= 5)
                {
                    bullets.Add(new Bullet(new Point(rocket.Rect.X + 50, rocket.Rect.Y + 30), new Point(15, 0), new Size(30, 30)));
                    LogAction?.Invoke("Произведен выстрел");
                }
            }
            if (e.KeyCode == Keys.Back)
            {
                SceneManager
                    .Get()
                    .Init<MenuScene>(form)
                    .Draw();
            }
        }

        private  void Timer_Tick(object sender, EventArgs e)
        {            
            Draw();
            Update();
        }

        public override void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);

            // Фон
            //Buffer.Graphics.FillRectangle(Brushes.Blue, new Rectangle(0, 0, Width, Height));
            Buffer.Graphics.DrawImage(Resources.background, new Rectangle(0, 0, Width, Height));

            foreach (var star in stars)
            {
                star.Draw();
            }

            // Планета
            //Buffer.Graphics.FillEllipse(Brushes.Red, new Rectangle(100, 100, 200, 200));
            Buffer.Graphics.DrawImage(Resources.planet, new Rectangle(100, 100, 200, 200));

            foreach (var asteroid in asteroids)
            {
                asteroid.Draw();
            }

            if (rocket != null)
            {
                rocket.Draw();
                Buffer.Graphics.DrawString($"Здоровье: {rocket.Energy}  |  Астероидов уничтожено: {asteroidCounter}", SystemFonts.DefaultFont, Brushes.White, 0, 0);
            }

            foreach (var bullet in bullets)
            {
                bullet.Draw();
            }

            Buffer.Render();
        }

        public  void Update()
        {
            foreach (var star in stars)
            {
                star.Update();
            }
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();
                if (bullets[i].Position.X > Width) bullets.Remove(bullets[i]);
            }

            for (int i = asteroids.Count - 1; i >= 0 ; i--)
            {
                asteroids[i].Update();
                bool asteroidDie = false;

                for (int j = bullets.Count - 1; j >= 0; j--)
                {
                    if (asteroids[i].Collision(bullets[j]))
                    {
                        asteroids.Remove(asteroids[i]);
                        bullets.Remove(bullets[j]);
                        asteroidDie = true;
                        LogAction?.Invoke("Попадание по астероиду");
                        asteroidCounter++;
                        break; 
                    }
                }

                if (!asteroidDie && rocket != null && asteroids[i].Collision(rocket))
                {
                    asteroids.Remove(asteroids[i]);
                    LogAction?.Invoke("Попадание астероидом в игрока");
                    rocket.EnergyLow(random.Next(10, 15));

                    if (rocket.Energy <= 0)
                    {
                        LogAction?.Invoke("Смерть игрока");
                        rocket.Die();
                    }
                }
            }
            if (asteroids.Count == 0) AsteroidCreation();
        }

        public void Load()
        {
            LogAction?.Invoke("Уровень загружен");

            bullets = new List<Bullet>();

            AsteroidCreation();
            
            rocket = new Rocket(new Point(0, Height / 2 - 50), new Point(15, 15), new Size(100, 100));
            rocket.DieEvent += Finish;            

            stars = new List<BaseObject>();
            for (int i = 0; i < 10; i++)
            {
                var size = random.Next(20, 30);
                stars.Add(new Star(new Point(600 - size * 20, i * 40 + 15), new Point(i + 1, i + 1), new Size(20, 20)));
            }
        }

        private void AsteroidCreation()
        {
            asteroids = new List<BaseObject>();

            for (int i = 0; i < startAsteroidCount; i++)
            {
                var size = random.Next(10, 40);
                asteroids.Add(new Asteroid(new Point(600 - size * 10, i * 20 + size), new Point(-i - 3, -i - 3), new Size(size, size)));
            }
            startAsteroidCount += 2;
        }

        private  void Finish(object sender, int e)
        {
            timer.Tick -= Timer_Tick;
            timer.Stop();
            Buffer.Graphics.DrawString($"Game Over! \n {e}", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }

        public override void Dispose()
        {
            base.Dispose();
            timer.Stop();
        }
    }
}
