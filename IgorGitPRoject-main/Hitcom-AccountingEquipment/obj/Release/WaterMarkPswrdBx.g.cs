﻿#pragma checksum "..\..\WaterMarkPswrdBx.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3FE88B3DAB9763DE8385877854619004C1B66BC0B6DFDBAE974D20C9741C5EFE"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Hitcom_AccountingEquipment;
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


namespace Hitcom_AccountingEquipment {
    
    
    /// <summary>
    /// WaterMarkPswrdBx
    /// </summary>
    public partial class WaterMarkPswrdBx : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\WaterMarkPswrdBx.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border brd;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\WaterMarkPswrdBx.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbWatermark;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\WaterMarkPswrdBx.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pb;
        
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
            System.Uri resourceLocater = new System.Uri("/Hitcom-AccountingEquipment;component/watermarkpswrdbx.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\WaterMarkPswrdBx.xaml"
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
            this.brd = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.tbWatermark = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.pb = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 17 "..\..\WaterMarkPswrdBx.xaml"
            this.pb.LostFocus += new System.Windows.RoutedEventHandler(this.PasswordLostFocus);
            
            #line default
            #line hidden
            
            #line 17 "..\..\WaterMarkPswrdBx.xaml"
            this.pb.GotFocus += new System.Windows.RoutedEventHandler(this.PasswordGotFocus);
            
            #line default
            #line hidden
            
            #line 18 "..\..\WaterMarkPswrdBx.xaml"
            this.pb.PasswordChanged += new System.Windows.RoutedEventHandler(this.Pb_OnPasswordChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

