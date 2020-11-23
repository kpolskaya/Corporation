﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corporation;

namespace WpfCorp.ViewModel
{
    /// <summary>
    /// Дополнительная обертка для департамента - нужна для специфических методов на уровне корпорации,
    /// также позволит работать с данными, если на верхнем уровне будет не один, а несколько департаментов
    /// (можно будет создать массив объектов - представлений департаментов и передать его как корневой элемент)
    /// </summary>
    public class CorporationViewModel
    {
        DepartmentViewModel board;
        ObservableCollection<DepartmentViewModel> firstTier;

        public CorporationViewModel(Department Board)
        {
            this.board = new DepartmentViewModel (Board);
            this.firstTier = new ObservableCollection<DepartmentViewModel>(
                new DepartmentViewModel[]
                {
                    this.board
                }
                );
            
        }

        //public DepartmentViewModel Board { get { return board; } }
        public ObservableCollection<DepartmentViewModel> FirstTier { get { return firstTier; } }

        public DepartmentViewModel SelectedItem
        {
            get
            {
                return Traverse(firstTier).FirstOrDefault<DepartmentViewModel>(i => i.IsSelected);
            }
        }

        private List<DepartmentViewModel> Traverse (Collection<DepartmentViewModel> children)
        {
            List<DepartmentViewModel> treeItems = new List<DepartmentViewModel>();

            foreach (var item in children)
            {
                treeItems.Add(item);

                if (item.Children != null && item.Children.Count >0)
                    treeItems.AddRange(Traverse(item.Children));
            }

            return treeItems;
        }

    }
}
