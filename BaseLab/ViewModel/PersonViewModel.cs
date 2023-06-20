using BaseLab.Helper;
using BaseLab.Model;
using BaseLab.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BaseLab.ViewModel
{
    internal class PersonViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Person> ListPerson { get; set; } = new ObservableCollection<Person>();
        public PersonViewModel()
        {
            this.ListPerson.Add(
            new Person
            {
                Id = 1,
                RoleId = 1,
                FirstName = "Иван",
                LastName = "Иванов",
                Birthday = new DateTime(1980, 02, 28)
            });
            this.ListPerson.Add(
            new Person
            {
                Id = 2,
                RoleId = 2,
                FirstName = "Петр",
                LastName = "Петров",
                Birthday = new DateTime(1981, 03, 20)
            });
            this.ListPerson.Add(
            new Person
            {
                Id = 3,
                RoleId = 3,
                FirstName = "Виктор",
                LastName = "Викторов",
                Birthday = new DateTime(1982, 04, 15)
            });
            this.ListPerson.Add(
            new Person
            {
                Id = 4,
                RoleId = 3,
                FirstName = "Сидор",
                LastName = "Сидоров",
                Birthday = new DateTime(1983, 05, 10)
            });

            ListPersonDpo = GetListPersonDpo();
        }
        public int MaxId()
        {
            int max = 0;
            foreach (var obPerson in this.ListPerson)
            {
                if (max < obPerson.Id)
                {
                    max = obPerson.Id;
                };
            }
            return max;
        }


        public ObservableCollection<PersonDPO> ListPersonDpo { get; set; } = new ObservableCollection<PersonDPO>();
        public ObservableCollection<PersonDPO> GetListPersonDpo()
        {
            foreach (var obPerson in ListPerson)
            {
                PersonDPO obPersonDpo = new PersonDPO();
                obPersonDpo = obPersonDpo.CopyFromPerson(obPerson);
                ListPersonDpo.Add(obPersonDpo);
            }
            return ListPersonDpo;
        }



        private PersonDPO selectedPersonDpo;
        public PersonDPO SelectedPersonDpo
        {
            get { return selectedPersonDpo; }
            set
            {
                selectedPersonDpo = value;
                OnPropertyChanged("SelectedPersonDpo");
            }
        }

        private RelayCommand addPerson;
        public RelayCommand AddPerson
        {
            get
            {
                return addPerson ??
                       (addPerson = new RelayCommand(obj =>
                       {
                           WindowNewEmployee wnPerson = new WindowNewEmployee
                           {
                               Title = "Новый сотрудник"
                           };

                           // формирование кода нового собрудника
                           int maxIdPerson = MaxId() + 1;
                           PersonDPO per = new PersonDPO
                           {
                               Id = maxIdPerson,
                               Birthday = DateTime.Now
                           };
                           wnPerson.DataContext = per;

                           if (wnPerson.ShowDialog() == true)
                           {
                               Role r = (Role)wnPerson.CbRole.SelectedValue;
                               per.RoleName = r.NameRole;
                               ListPersonDpo.Add(per);

                               // добавление нового сотрудника в коллекцию ListPerson<Person> 
                               Person p = new Person();
                               p = p.CopyFromPersonDPO(per);
                               ListPerson.Add(p);
                           }
                       },
                           (obj) => true));
            }
        }


        private RelayCommand editPerson;
        public RelayCommand EditPerson
        {
            get
            {
                return editPerson ??
                       (editPerson = new RelayCommand(obj =>
                       {
                           WindowNewEmployee wnPerson = new WindowNewEmployee()
                           {
                               Title = "Редактирование данных сотрудника",
                           };

                           PersonDPO personDpo = SelectedPersonDpo;
                           PersonDPO tempPerson = new PersonDPO();
                           tempPerson = personDpo.ShallowCopy();
                           wnPerson.DataContext = tempPerson;

                           //wnPerson.CbRole.ItemsSource = new ListRole();
                           if (wnPerson.ShowDialog() == true)
                           {
                               // сохранение данных в оперативной памяти
                               // перенос данных из временного класса в класс отображения данных  
                               Role r = (Role)wnPerson.CbRole.SelectedValue;
                               personDpo.RoleName = r.NameRole;
                               personDpo.FirstName = tempPerson.FirstName;
                               personDpo.LastName = tempPerson.LastName;
                               personDpo.Birthday = tempPerson.Birthday;


                               // перенос данных из класса отображения данных в класс Person
                               FindPerson finder = new FindPerson(personDpo.Id);
                               List<Person> listPerson = ListPerson.ToList();
                               Person p = listPerson.Find(new Predicate<Person>(finder.PersonPredicate));
                               p = p.CopyFromPersonDPO(personDpo);
                           }
                       }, (obj) => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
            }
        }

        private RelayCommand deletePerson;
        public RelayCommand DeletePerson
        {
            get
            {
                return deletePerson ??
                       (deletePerson = new RelayCommand(obj =>
                       {
                           PersonDPO person = SelectedPersonDpo;

                           MessageBoxResult result = 
                           MessageBox.Show("Удалить данные по сотруднику: \n" + person.LastName + " " + person.FirstName,
                               "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                           if (result == MessageBoxResult.OK)
                           {
                               // удаление данных в списке отображения данных
                               ListPersonDpo.Remove(person);

                               // удаление данных в списке классов ListPerson<Person>
                               Person per = new Person();
                               per = per.CopyFromPersonDPO(person);
                               ListPerson.Remove(per);
                           }

                       }, (obj) => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
