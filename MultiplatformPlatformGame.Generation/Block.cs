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
		public List<Component> ComponentMatrix { get; set; }

		public void BindComponents(List<Component> components)
		{
			ComponentMatrix = new List<Component> ();

			foreach (int id in Matrix) {
				bool found = false;
				foreach (Component c in components) {
					if (c.Id == id) {
						ComponentMatrix.Add (c);
						found = true;
					}
				}
				if (!found) {
					throw new KeyNotFoundException (String.Format("Component id {0} specified in Block id {1} not registered", id, Id));
				}
			}
		}
	}
}