///Copyright(c) 2015,HIT All rights reserved.
///Summary:Singularity interface
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      
      
using System.Collections.Generic;

namespace Irlovan.Register
{
    public interface ISingularity : IUnit
    {

        #region Property

        /// <summary>
        /// Singularity of the Universe
        /// </summary>
        IUniverse Container { get; }

        #endregion Property

    }
}
