﻿

#pragma checksum "C:\Users\Konstantinos\SkyDrive\Documents\Visual Studio\Projects\Bequized\Bequized\MainMenu.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0270E664976892EE1DFA2DE28B4D6F07"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bequized
{
    partial class MainMenu : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 571 "..\..\MainMenu.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.StartGameBtn_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 572 "..\..\MainMenu.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.TimedGameStartBtn_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 574 "..\..\MainMenu.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.TestClick;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


