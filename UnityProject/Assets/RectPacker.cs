using System;
using System.Collections.Generic;
using UnityEngine;

internal class RectPacker
{
	private readonly System.Random random;
	private readonly List<Rect> rectTemp = new List<Rect>();
	private readonly List<Rect> rectAvail = new List<Rect>();
	private readonly List<TileCoordinates>[] coords = new List<TileCoordinates>[] {
		new List<TileCoordinates>(), new List<TileCoordinates>(), new List<TileCoordinates>()
	};
	private Rect bounds;

	public RectPacker(System.Random random)
	{
		this.random = random;
	}

	private struct Rect
	{
		public Rect(int x0, int y0, int x1, int y1)
		{
			this.x0 = x0;
			this.y0 = y0;
			this.x1 = x1;
			this.y1 = y1;
		}
		public int x0, y0, x1, y1;
	}
	
	public void Clear(int x, int y, int width, int height)
	{
		// Debug.Log(string.Format("Clear: {0} {1} {2} {3}", x, y, width, height));
		this.bounds = new Rect(x, y, x + width, y + height);
		this.rectAvail.Clear();
		this.rectAvail.Add(this.bounds);
	}

	private void AddCoordinate(int x, int y, int size)
	{
		bool left = x == this.bounds.x0, right = x + size == this.bounds.x1;
		bool down = y == this.bounds.y0, up = y + size == this.bounds.y1;
		int priority;
		if ((down || up) && (left || right))
			priority = 0;
		else if (down || up || left || right)
			priority = 1;
		else
			priority = 2;
		List<TileCoordinates> list = this.coords[priority];
		foreach (TileCoordinates c in list) {
			if (c.x == x && c.y == y)
				continue;
		}
		list.Add(new TileCoordinates(x, y));
	}

	public TileCoordinates? PlaceRect(int size)
	{
		// Debug.Log(string.Format("PlaceRect: {0}", size));
		foreach (List<TileCoordinates> list in this.coords)
			list.Clear();
		foreach (Rect r in this.rectAvail) {
			if (size > r.x1 - r.x0 || size > r.y1 - r.y0)
				continue;
			this.AddCoordinate(r.x0, r.y0, size);
			if (size < r.x1 - r.x0)
				this.AddCoordinate(r.x1 - size, r.y0, size);
			if (size < r.y1 - r.y0)
				this.AddCoordinate(r.x0, r.y1 - size, size);
			if (size < r.x1 - r.x0 && size < r.y1 - r.y0)
				this.AddCoordinate(r.x1 - size, r.y1 - size, size);
		}

		for (int priority = 0; priority < 3; priority++) {
			List<TileCoordinates> list = this.coords[priority];
			if (list.Count == 0)
				continue;
			this.rectTemp.Clear();
			TileCoordinates loc = list[this.random.Next(list.Count)];
			int x0 = loc.x, x1 = loc.x + size, y0 = loc.y, y1 = loc.y + size;
			foreach (Rect r in this.rectAvail) {
				bool isSplit = true;
				if (y0 >= r.y1 || y1 <= r.y0 || x0 >= r.x1 || x1 <= r.x0) {
					this.rectTemp.Add(r);
					continue;
				}
				if (x0 > r.x0 && x0 < r.x1)
					this.rectTemp.Add(new Rect(r.x0, r.y0, x0, r.y1));
				if (x1 > r.x0 && x1 < r.x1)
					this.rectTemp.Add(new Rect(x1, r.y0, r.x1, r.y1));
				if (y0 > r.y0 && y0 < r.y1)
					this.rectTemp.Add(new Rect(r.x0, r.y0, r.x1, y0));
				if (y1 > r.y0 && y1 < r.y1)
					this.rectTemp.Add(new Rect(r.x0, y1, r.x1, r.y1));
			}
			int n = this.rectTemp.Count;
			this.rectAvail.Clear();
			for (int i = 0; i < n; i++) {
				Rect r = this.rectTemp[i];
				bool emit = true;
				for (int j = 0; j < n; j++) {
					Rect s = this.rectTemp[j];
					if (r.x0 >= s.x0 && r.x1 <= s.x1 && r.y0 >= s.y0 && r.y1 <= s.y1) {
						if (r.x0 == s.x0 && r.x1 == s.x1 && r.y0 == s.y0 && r.y1 == s.y1) {
							if (i < j) {
								emit = false;
								break;
							}
						} else {
							emit = false;
							break;
						}
					}
				}
				if (emit)
					this.rectAvail.Add(r);
			}

			return loc;
		}

		return null;
	}
}
