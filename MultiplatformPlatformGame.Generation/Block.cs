using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MultiplatformPlatformGame.Generation
{
    [DataContract]
	public class Block
	{
		public Block ()
		{
		}
        [DataMember(Name = "id")]        
		public int Id { get; set; }
        [DataMember(Name = "name")]
        public String Name { get; set; }
        [DataMember(Name = "matrix")]
        public List<int> Matrix { get; set; }

        public List<List<BlockComponent>> ComponentMatrix { get; set; }
        public int BlockSize { get; set; }

		public void BindComponents(List<Component> components)
		{
			ComponentMatrix = new List<List<BlockComponent>> ();

			int BlockSize = Convert.ToInt32(Math.Sqrt (Matrix.Count));
			int counter = 0;
			List<BlockComponent> row = null;

			foreach (int id in Matrix) {
				if (counter % BlockSize == 0) {
					row = new List<BlockComponent> ();
					ComponentMatrix.Add (row);
				}

				bool found = false;
				foreach (Component c in components) {
					if (c.Id == id) {
						BlockComponent bc = new BlockComponent(c, this, counter / BlockSize, counter % BlockSize);
						row.Add (bc);
						found = true;
					}
				}
				if (!found) {
					throw new KeyNotFoundException (String.Format("Component id {0} specified in Block id {1} not registered", id, Id));
				}

				counter++;
			}
		}
	}
}