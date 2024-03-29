﻿using Production_Facility.Models;
using Production_Facility.ViewModels.Commands;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;

namespace Production_Facility.ViewModels
{
    public class RecipeViewModel : INotifyPropertyChanged
    {
        FacilityDBContext dbContext = new FacilityDBContext();

        private ObservableCollection<Recipe.RecipeLine> lista = new ObservableCollection<Recipe.RecipeLine>();

        private string name = "Receptury";
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        public RecipeViewModel()
        {
            
            ComboBoxLoader = new RelayCommand(SetComboBox);
            DataGridLoader = new RelayCommand(SetRecipe);
        }
        

        public ICommand DataGridLoader { get; set; }
        public ICommand ComboBoxLoader { get; set; }

        public void SetComboBox (object obj)
        {
            string s = obj as string;

            List<Item> entities = (from q in dbContext.Items
                                   from x in dbContext.Recipes
                                   where q.Name.Contains(s)
                                   where x.RecipeOwner == q.Number
                                   select q).ToList();

            UserChoice = entities;
        }

        public void SetRecipe(object obj)
        {
            string s = obj as string;

            var recipe = (from xx in dbContext.Recipes
                           where xx.RecipeOwner == s
                           select xx).FirstOrDefault<Recipe>();

            if (recipe != null)
            {
                lista.Clear();

                string[] temp = recipe.RecipeComposition.Split('|');

                foreach (string str in temp)
                {
                    string[] temp2 = str.Split('=');
                    Recipe.RecipeLine newRecipeLine = new Recipe.RecipeLine(int.Parse(temp2[0]), temp2[1], double.Parse(temp2[2]));

                    lista.Add(newRecipeLine);
                }

                ItemRecipe = lista;
                //OnPropertyChanged("ItemRecipe");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private ObservableCollection<Recipe.RecipeLine> itemRecipe = new ObservableCollection<Recipe.RecipeLine>();

        public ObservableCollection<Recipe.RecipeLine> ItemRecipe
        {
            get { return itemRecipe; }
            set
            {
                itemRecipe = value;
                OnPropertyChanged("ItemRecipe");
            }
        }

        private List<Item> userChoice = new List<Item>();

        public List<Item> UserChoice
        {
            get { return userChoice; }
            set
            {
                userChoice = value;
                OnPropertyChanged("UserChoice");
            }
        }
    }
}
