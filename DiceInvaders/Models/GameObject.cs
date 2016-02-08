using System.ComponentModel;
using DiceInvaders.Annotations;

namespace DiceInvaders.Models
{
    public class GameObject:INotifyPropertyChanged
    {

        private int _left;
        private int _top;
        public GameObjectType Type { get; set; }
        public string Sprite { get; set; }
        public int Left
        {
            get
            {
                return _left;
            }
            set
            {
                if (_left != value)
                {
                    _left = value;
                    OnPropertyChanged("Left");
                }
            }
        }
        public int Top
        {
            get
            {
                return _top;
            }
            set
            {
                if (_top != value)
                {
                    _top = value;
                    OnPropertyChanged("Top");
                }
            }
        }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
