using System;

namespace Aya.DataBinding
{
    public interface IBindable<T>
    {
        T Value { get; set; }
        Action<T> OnValueChanged { get; set; }
    }

    public class BindableValue<T>: IBindable<T>
    {
        public T Value
        {
            get => InternalValue;
            set
            {
                if (InternalValue.Equals(value)) return;
                InternalValue = value;
                OnValueChanged?.Invoke(InternalValue);
            }
        }

        protected T InternalValue;

        public Action<T> OnValueChanged { get; set; } = delegate { };
    }

    public class Test
    {
        public BindableValue<float> A;

        public Test()
        {
            var binder = new BindableValueBinder<float>(A);
        }
    }

    public class BindableValueBinder<TData> : DataBinder<IBindable<TData>, TData>
    {
        public override void AddListener() => Target.OnValueChanged += OnValueChangedCallback;
        public override void RemoveListener() => Target.OnValueChanged -= OnValueChangedCallback;

        public BindableValueBinder(IBindable<TData> target)
        {
            Target = target;
        }

        public override void SetData(TData data)
        {
            Target.Value = data;
        }

        public override TData GetData()
        {
            return Target.Value;
        }
    }
}

