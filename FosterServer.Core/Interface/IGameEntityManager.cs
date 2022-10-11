using FosterServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.Core.Interface
{
    public interface IGameEntityManager
    {
        void Initialize(float a_X, float a_Y, Size a_entitySize, float a_rotation = 0, bool a_isInteractable = false, bool a_canEntityPassThrough = false);

        void InitializeInventory();

        void ToggleCanEntityBePassedThrough();

        void SetPosition(Vector3 a_position);

        void SetPosition(float a_x, float a_y, float a_z = 1);

        void SetRotation(float a_rotate);

        bool HasGridPointIntersect(GridPoint a_sourcePoint);

        void AddGameProperty(string a_propertyName, object a_defaultValue);

        void SetGameProperty(string a_propertyName, object a_value);

        void ChangeValueGameProperty(string a_propertyName, object a_value);

        object GetGameProperty(string a_propertyName);
    }
}
