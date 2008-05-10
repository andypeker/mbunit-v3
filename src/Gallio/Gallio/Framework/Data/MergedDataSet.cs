// Copyright 2005-2008 Gallio Project - http://www.gallio.org/
// Portions Copyright 2000-2004 Jonathan De Halleux, Jamie Cansdale
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using Gallio.Collections;

namespace Gallio.Framework.Data
{
    /// <summary>
    /// <para>
    /// A merged data set is an aggregate data set that selects rows from each of zero or more
    /// other data sets according to a <see cref="IMergeStrategy"/>.
    /// </para>
    /// </summary>
    public class MergedDataSet : AggregateDataSet
    {
        private IMergeStrategy strategy = ConcatenationMergeStrategy.Instance;
        private int columnCount;

        /// <summary>
        /// Gets or sets the <see cref="IMergeStrategy"/>.
        /// By default the strategy is <see cref="ConcatenationMergeStrategy"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
        public IMergeStrategy Strategy
        {
            get { return strategy; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                strategy = value;
            }
        }

        /// <inheritdoc />
        public override int ColumnCount
        {
            get { return columnCount; }
        }

        /// <inheritdoc />
        public override void AddDataSet(IDataSet dataSet)
        {
            base.AddDataSet(dataSet);
            columnCount = Math.Max(columnCount, dataSet.ColumnCount);
        }

        /// <inheritdoc />
        protected override bool CanBindImpl(DataBinding binding)
        {
            foreach (IDataSet dataSet in DataSets)
            {
                if (!dataSet.CanBind(binding))
                    return false;
            }

            return true;
        }

        /// <inheritdoc />
        protected override IEnumerable<IDataRow> GetRowsImpl(ICollection<DataBinding> bindings, bool includeDynamicRows)
        {
            IDataProvider[] providers = GenericUtils.ToArray(DataSets);
            return strategy.Merge(providers, bindings, includeDynamicRows);
        }
    }
}