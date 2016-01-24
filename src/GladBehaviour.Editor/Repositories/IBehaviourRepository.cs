using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladBehaviour.Editor
{
	/// <summary>
	/// Implementers provides model production/building/provider services.
	/// </summary>
	public interface IBehaviourRepository
	{
		/// <summary>
		/// Generates a collection of models.
		/// </summary>
		/// <returns></returns>
		IEnumerable<IDataStoreModel> BuildModels();
	}
}
