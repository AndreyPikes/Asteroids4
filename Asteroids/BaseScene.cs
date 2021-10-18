using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asteroids
{
    public abstract class BaseScene : IScene, IDisposable
    {
        protected BufferedGraphicsContext _context;
        protected Form form;
        public static BufferedGraphics Buffer;
        public string SceneType { get { return (this.GetType().Name); } }

        public event Action<string> Log;

        public static int Width { get; set; }
        public static int Height { get; set; }

        public virtual void Init(Form form)
        {
            Log?.Invoke("Запуск");

            this.form = form;
            Graphics g;
            _context = BufferedGraphicsManager.Current;
            g = this.form.CreateGraphics();
            Width = this.form.ClientSize.Width;
            Height = this.form.ClientSize.Height;
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            this.form.KeyDown += SceneKeyDown;
        }

        public virtual void SceneKeyDown(object sender, KeyEventArgs e) { }

        public virtual void Draw() { }

        public virtual void Dispose()
        {
            Buffer = null;
            _context = null;
            form.KeyDown -= SceneKeyDown;
        }
    }
}
