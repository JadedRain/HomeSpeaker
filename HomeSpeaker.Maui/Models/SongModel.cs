using HomeSpeaker.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSpeaker.Maui.Models;

public class SongModel
{
	public int SongId { get; set; }
	public required string Name { get; init; }
	private string? path;
	public string? Path
	{
		get => path;
		set
		{
			path = value;
			if (path?.Contains('\\') ?? false)
				Folder = System.IO.Path.GetDirectoryName(path.Replace('\\', '/'));
			else
				Folder = System.IO.Path.GetDirectoryName(path);
		}
	}
	public required string Album { get; init; }
	public required string Artist { get; init; }
	public string? Folder { get; private set; }
}


public static class ViewModelExtensions
{
	public static SongModel ToSongModel(this SongMessage song)
	{
		return new SongModel
		{
			SongId = song?.SongId ?? -1,
			Name = song?.Name?.Trim() ?? "[ Null Song Response ??? ]",
			Album = song?.Album?.Trim() ?? "[ No Album ]",
			Artist = song?.Artist?.Trim() ?? "[ No Artist ]",
			Path = song?.Path?.Trim()
		};
	}
}
