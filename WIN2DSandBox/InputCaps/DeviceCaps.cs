using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Devices.Input;
using WIN2DSandBox.Annotations;

namespace WIN2DSandBox.InputCaps
{
    public abstract class DeviceCaps : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isPresent;

        public bool IsPresent
        {
            get
            {
                return _isPresent; 
            }
            set
            {
                _isPresent = value;
                OnPropertyChanged("IsPresent");
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class DeviceCapsVM: INotifyPropertyChanged
    {
        private MouseCaps _mouseCapabilities = new MouseCaps();
        private KeyboardCaps _keyboardCapabilities = new KeyboardCaps();
        private TouchCaps _touchCapabilities = new TouchCaps();
        
        public MouseCaps MouseCapabilities
        {
            get { return _mouseCapabilities; }
            set
            {
                _mouseCapabilities = value; 
                OnPropertyChanged(nameof(MouseCapabilities));
            }
        }

        public KeyboardCaps KeyboardCapabilities
        {
            get { return _keyboardCapabilities; }
            set
            {
                _keyboardCapabilities = value; 
                OnPropertyChanged(nameof(KeyboardCapabilities));
            }
        }

        public TouchCaps TouchCapabilities
        {
            get { return _touchCapabilities; }
            set
            {
                _touchCapabilities = value;
                OnPropertyChanged(nameof(TouchCapabilities));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MouseCaps : DeviceCaps
    {
        private bool _horizontalWheelPresent;
        private bool _verticalWheelPresent;
        private uint _numberOfButtons;
        private bool _swapButtons;

        public bool HorizontalWheelPresent
        {
            get { return _horizontalWheelPresent; }
            set
            {
                _horizontalWheelPresent = value;
                OnPropertyChanged("HorizontalWheelPresent");
            }
        }

        public bool VerticalWheelPresent
        {
            get { return _verticalWheelPresent; }
            set
            {
                _verticalWheelPresent = value;
                OnPropertyChanged("VerticalWheelPresent");
            }
        }

        public uint NumberOfButtons
        {
            get { return _numberOfButtons; }
            set
            {
                _numberOfButtons = value;
                OnPropertyChanged("NumberOfButtons");
            }
        }

        public bool SwapButtons

        {
            get { return _swapButtons; }
            set
            {
                _swapButtons = value;
                OnPropertyChanged("SwapButtons");
            }
        }

        public MouseCaps()
        {
            var mouseCaps = new MouseCapabilities();
            HorizontalWheelPresent = mouseCaps.HorizontalWheelPresent != 0;
            VerticalWheelPresent = mouseCaps.VerticalWheelPresent != 0;
            IsPresent = mouseCaps.MousePresent != 0;
            NumberOfButtons = mouseCaps.NumberOfButtons;
            SwapButtons = mouseCaps.SwapButtons != 0;
        }



    }
    public class KeyboardCaps : DeviceCaps
    {
        public KeyboardCaps()
        {
            var keybCaps = new KeyboardCapabilities();
            IsPresent = keybCaps.KeyboardPresent != 0;
        }
    }

    public class TouchCaps : DeviceCaps
    {
        private uint _numberOfcontacts;
        public uint NumberOfcontacts
        {
            get { return _numberOfcontacts; }
            set
            {
                _numberOfcontacts = value;
                OnPropertyChanged("NumberOfcontacts");
            }
        }

        public TouchCaps()
        {
            var touchCaps = new TouchCapabilities();
            IsPresent = touchCaps.TouchPresent != 0;
            NumberOfcontacts = touchCaps.Contacts;
        }
    }
}

