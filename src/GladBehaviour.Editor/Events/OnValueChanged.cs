using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Editor
{
	/// <summary>
	/// Action signature for events related to behaviour component values changing.
	/// </summary>
	/// <param name="sender">Object that raised the event.</param>
	/// <param name="args">Change arguments.</param>
	public delegate void OnValueChanged(object sender, GladBehaviourValueChangedArgs args);
}
