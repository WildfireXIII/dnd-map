﻿#pragma checksum "..\..\MapDesignWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "16D372A7D4CB89B0731ECC8E80E2ABE8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace DNDMapMaker {
    
    
    /// <summary>
    /// MapDesignWindow
    /// </summary>
    public partial class MapDesignWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\MapDesignWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas cnvsWorld;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\MapDesignWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbRes;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\MapDesignWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbEntities;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\MapDesignWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle rPreviewPane;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\MapDesignWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer svDebug;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\MapDesignWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblDebug;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DNDMapMaker;component/mapdesignwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MapDesignWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cnvsWorld = ((System.Windows.Controls.Canvas)(target));
            
            #line 19 "..\..\MapDesignWindow.xaml"
            this.cnvsWorld.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.cnvsWorld_MouseDown);
            
            #line default
            #line hidden
            
            #line 19 "..\..\MapDesignWindow.xaml"
            this.cnvsWorld.MouseMove += new System.Windows.Input.MouseEventHandler(this.cnvsWorld_MouseMove);
            
            #line default
            #line hidden
            
            #line 19 "..\..\MapDesignWindow.xaml"
            this.cnvsWorld.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.cnvsWorld_MouseUp);
            
            #line default
            #line hidden
            
            #line 19 "..\..\MapDesignWindow.xaml"
            this.cnvsWorld.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.cnvsWorld_MouseWheel);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 37 "..\..\MapDesignWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.lbRes = ((System.Windows.Controls.ListBox)(target));
            
            #line 51 "..\..\MapDesignWindow.xaml"
            this.lbRes.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lbRes_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.lbEntities = ((System.Windows.Controls.ListBox)(target));
            return;
            case 5:
            this.rPreviewPane = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 6:
            this.svDebug = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 7:
            this.lblDebug = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

