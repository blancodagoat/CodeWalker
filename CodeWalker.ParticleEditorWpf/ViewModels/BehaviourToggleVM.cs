using CodeWalker.GameFiles;
using System;

namespace CodeWalker.ParticleEditorWpf.ViewModels
{
    // A single behaviour on/off toggle in the behaviour grid (Age, Velocity, Size, Colour, ...).
    public class BehaviourToggleVM : ViewModelBase
    {
        public ParticleBehaviourType Type { get; }
        public string Name { get; }

        private readonly Action<BehaviourToggleVM, bool> onToggle;
        private bool present;

        public bool Present
        {
            get => present;
            set
            {
                if (present == value) return;
                present = value;
                OnPropertyChanged();
                onToggle?.Invoke(this, value);
            }
        }

        // sets the backing value without firing the toggle action (for refreshing from the data)
        public void SetPresentQuiet(bool value)
        {
            if (present == value) return;
            present = value;
            OnPropertyChanged(nameof(Present));
        }

        public BehaviourToggleVM(ParticleBehaviourType type, bool present, Action<BehaviourToggleVM, bool> onToggle)
        {
            Type = type;
            Name = type.ToString();
            this.present = present;
            this.onToggle = onToggle;
        }
    }
}
