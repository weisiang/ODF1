using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonData.HIRATA
{
    public class CommonDatabase
    {
        protected object cv_obj = new object();
        protected string cv_FilePath = "";
        protected bool cv_IsAutoSave = false;
        public bool PIsAutoSave
        {
            get { return cv_IsAutoSave; }
            set { cv_IsAutoSave = value; }
        }
    }
}
