using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skyunion
{
    public abstract class DBRecord
    {
        public abstract void Parse(string[] cols);
        protected object [] fileds;
    }
}