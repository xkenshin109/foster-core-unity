using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.Core.Models
{
    public class Point
    {
        private float m_x, m_y;

        public float X { get { return m_x; } }

        public float Y { get { return m_y; } }
        public Point(float a_x, float a_y)
        {
            m_x = a_x;
            m_y = a_y;
        }

        public Point(Point a_point)
        {
            m_x = a_point.X;
            m_y = a_point.Y;
        }

        public Point(int a_x, int a_y)
        {
            m_x = a_x;
            m_y = a_y;
        }

        public Point(Vector3 a_position)
        {
            m_x = a_position.x;
            m_y = a_position.y;
        }

        public void SetPosition(Vector3 a_position)
        {
            m_x = a_position.x;
            m_y = a_position.y;
        }
    }
}
