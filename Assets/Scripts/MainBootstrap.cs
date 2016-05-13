using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

namespace rot.main
{
    public class MainBootstrap : ContextView
    {
        private void Awake()
        {
            if (Context.firstContext == null)
            {
                context = new MainContext(this);
                ContextView.obj = this.gameObject;
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}