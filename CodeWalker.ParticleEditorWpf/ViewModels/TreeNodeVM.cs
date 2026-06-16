using CodeWalker.GameFiles;
using System;
using System.Collections.ObjectModel;

namespace CodeWalker.ParticleEditorWpf.ViewModels
{
    public enum NodeKind { Effect, Emitter, EmitterRule, ParticleRule, Behaviour, Domain }

    // A node in the effects browser tree. Wraps an underlying particle data object for the property grid + preview.
    public class TreeNodeVM : ViewModelBase
    {
        public string Header { get; set; }
        public NodeKind Kind { get; }
        public object Data { get; }                 //the underlying particle object (bound into the property grid)
        public ParticleEffectRule OwnerEffect { get; }
        public ParticleRule OwnerParticleRule { get; }
        public ParticleEventEmitter OwnerEmitter { get; }
        public ObservableCollection<TreeNodeVM> Children { get; } = new ObservableCollection<TreeNodeVM>();

        private bool isExpanded = true;
        public bool IsExpanded { get => isExpanded; set => SetField(ref isExpanded, value); }

        private bool isSelected;
        public bool IsSelected { get => isSelected; set => SetField(ref isSelected, value); }

        public TreeNodeVM(string header, NodeKind kind, object data,
            ParticleEffectRule ownerEffect = null, ParticleRule ownerParticleRule = null, ParticleEventEmitter ownerEmitter = null)
        {
            Header = header;
            Kind = kind;
            Data = data;
            OwnerEffect = ownerEffect;
            OwnerParticleRule = ownerParticleRule;
            OwnerEmitter = ownerEmitter;
        }

        // --- presentation helpers for the browser tree ---

        // the name with the redundant "Emitter: " / "Particle Rule: " etc. prefix stripped (it's shown as a tag instead)
        public string DisplayName
        {
            get
            {
                if (Kind != NodeKind.Effect && !string.IsNullOrEmpty(Header))
                {
                    int i = Header.IndexOf(": ", StringComparison.Ordinal);
                    if (i >= 0) return Header.Substring(i + 2);
                }
                return Header;
            }
        }

        public string TypeTag => Kind switch
        {
            NodeKind.Effect => "EFFECT",
            NodeKind.Emitter => "EMITTER",
            NodeKind.EmitterRule => "EMITTER RULE",
            NodeKind.ParticleRule => "PARTICLE",
            NodeKind.Behaviour => "BEHAVIOUR",
            NodeKind.Domain => "DOMAIN",
            _ => "",
        };

        public string AccentColor => Kind switch
        {
            NodeKind.Effect => "#E3B341",       // amber - top level
            NodeKind.Emitter => "#4FB0E0",      // blue
            NodeKind.EmitterRule => "#7A8290",  // slate
            NodeKind.ParticleRule => "#8FD06A", // green
            NodeKind.Behaviour => "#C678DD",    // purple
            NodeKind.Domain => "#E0884F",       // orange
            _ => "#808080",
        };

        public bool IsEffect => Kind == NodeKind.Effect;
        public string NameColor => IsEffect ? "#F4F4F4" : "#D2D2D2";
        public string NameWeight => IsEffect ? "SemiBold" : "Normal";

        // emitter visibility toggle (shown only on Emitter rows)
        public bool IsEmitter => Kind == NodeKind.Emitter;
        public Action<TreeNodeVM, bool> VisibilityToggle;
        private bool isVisible = true;
        public bool IsVisible
        {
            get => isVisible;
            set { if (SetField(ref isVisible, value)) VisibilityToggle?.Invoke(this, value); }
        }
    }
}
