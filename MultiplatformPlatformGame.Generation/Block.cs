using System;
using System.Collections.Generic;

namespace MultiplatformPlatformGame.Generation
{
	public class Block
	{
		public Block ()
		{
		}

		public int Id { get; set; }
		public String Name { get; set; }
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

		public void PrettyPrint()
		{
			foreach (List<BlockComponent> l in this.ComponentMatrix) {
				foreach (BlockComponent bc in l) {
					if (bc.Component.Solid) {
						Console.Write ('X');
					} else {
						Console.Write (' ');
					}
				}
				Console.WriteLine ();
			}
		}
	}
}