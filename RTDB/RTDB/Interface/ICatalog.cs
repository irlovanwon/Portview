///Copyright(c) 2015,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2015-01-29
///Description:
///Modification:2015-11-06*************Add RemoveChild Function

using System.Collections.Generic;

namespace Irlovan.Database
{
    public interface ICatalog : IDatabase
    {

        /// <summary>
        /// Add Child Element to a calalog
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        void Add(IDatabase child);

        /// <summary>
        /// Remove Child element
        /// </summary>
        /// <param name="name"></param>
        bool RemoveChild(string name);

        /// <summary>
        /// Acquire data by name from catalog
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDatabase AcquireData(string name);

        /// <summary>
        /// Acquire industrydata by name from catalog
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IIndustryData<T> AcquireIndustryData<T>(string name);


        /// <summary>
        /// Acquire industrydata by name from catalog
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IIndustryData AcquireIndustryData(string name);

        /// <summary>
        /// Acquire catalog by name from catalog
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ICatalog AcquireCatalog(string name);

        /// <summary>
        /// Acquire eventdata by name from catalog
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEventData AcquireEventData(string name);

        /// <summary>
        /// Acquire all industry data
        /// </summary>
        /// <param name="includeChild">Is child Catalog included</param>
        /// <returns></returns>
        List<IIndustryData> AcquireAll(bool includeChild);

    }

}
