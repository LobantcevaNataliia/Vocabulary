﻿#pragma checksum "..\..\Exercise.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5DEFCADD37CF29AD7E6CA88CF53C47AB38EAB7A0DD524EEA148623601A8EB58C"
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
using System.Windows.Forms.Integration;
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
using Vocabulary;


namespace Vocabulary {
    
    
    /// <summary>
    /// Exercise
    /// </summary>
    public partial class Exercise : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\Exercise.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Vocabulary.Exercise ExerciseWindow;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\Exercise.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Learn;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\Exercise.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Translation;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\Exercise.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Repeat;
        
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
            System.Uri resourceLocater = new System.Uri("/Vocabulary;component/exercise.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Exercise.xaml"
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
            this.ExerciseWindow = ((Vocabulary.Exercise)(target));
            
            #line 8 "..\..\Exercise.xaml"
            this.ExerciseWindow.Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 8 "..\..\Exercise.xaml"
            this.ExerciseWindow.Closed += new System.EventHandler(this.ExerciseWindow_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Learn = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\Exercise.xaml"
            this.Learn.Click += new System.Windows.RoutedEventHandler(this.Learn_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Translation = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\Exercise.xaml"
            this.Translation.Click += new System.Windows.RoutedEventHandler(this.Translation_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Repeat = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\Exercise.xaml"
            this.Repeat.Click += new System.Windows.RoutedEventHandler(this.Repeat_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

