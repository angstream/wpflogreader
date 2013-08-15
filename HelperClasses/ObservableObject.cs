﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace WPFReader
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        /// <summary>    /// Raised when a property on this object has a new value.           /// </summary>    
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;     /// <summary>    
        /// Raises this object's PropertyChanged event.    /// </summary>    
        /// <param name="propertyName">The property that has a new value.</param>    
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName); 
            if (this.PropertyChanged != null) { 
                var e = new PropertyChangedEventArgs(propertyName); 
                this.PropertyChanged(this, e); 
            }
        }

        #endregion // Debugging Aides}

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public virtual void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,        // public, instance property on this object.        
            if (TypeDescriptor.GetProperties(this)[propertyName] == null) { 
                string msg = "Invalid property name: " + propertyName; 
                if (this.ThrowOnInvalidPropertyName)                
                    throw new Exception(msg); 
                else                
                    Debug.Fail(msg); 
            }
        }
    }
}
