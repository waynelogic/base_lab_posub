﻿using BaseLab.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BaseLab.View
{
    /// <summary>
    /// Логика взаимодействия для WindowRole.xaml
    /// </summary>
    public partial class WindowRole : Window
    {
        private RoleViewModel vmRole;
        public WindowRole()
        {
            InitializeComponent();
            RoleViewModel vmRole = new RoleViewModel();
            lvRole.ItemsSource = vmRole.ListRole;
            this.vmRole = vmRole;
        }
    }
}
