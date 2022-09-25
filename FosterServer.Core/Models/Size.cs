using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Models
{
    public class Size
    {
        private float m_width, m_height;

        public float Width { get { return m_width; } }
        public float Height { get { return m_height; } }

        public Size(float a_width, float a_height)
        {
            m_width = a_width;
            m_height = a_height;
        }

        public Size(Size a_size)
        {
            m_width = a_size.Width;
            m_height = a_size.Height;
        }

        public Size(int a_width, int a_height)
        {
            m_width = a_width;
            m_height = a_height;
        }
    }
}
