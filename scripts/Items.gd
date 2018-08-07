extends Node


var SteelGame = null


class ItemInstance:
	var name = 'Error'
	var temp = 0
	var count = 0
	var uses = 0

class ItemInfo:
	var name = 'Error'
	var type = 'Error'
	var description = 'This is an error item and should not exist.'


func return_instance(name):
	var out = ItemInstance.new()
	out.name = name
	out.temp = 0
	out.count = 1
	out.uses = 0
	return out

func lookup(name):
	var out = ItemInfo.new()
	match name:
		'Platform':
			out.name = 'Platform'
			out.type = 'Structure'
			out.description = 'Basic flat platform, for use as a foor or ceiling.'

	return out

func thumbnail(name):
	var out = load("res://textures/thumbnails/" + name + '.png')

	if out == null:
		out = load("res://textures/error.png")
		SConsole.logf('Error: Failed to find thumbnail for item "' + name + '"')

	return out

func _ready():
	self.SteelGame = get_tree().get_root().get_node("SteelGame")
