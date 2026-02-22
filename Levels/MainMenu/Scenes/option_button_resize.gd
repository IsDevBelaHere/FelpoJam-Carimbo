extends OptionButton


# Called when the node enters the scene tree for the first time.
@export
var fontstyle : FontFile

func _ready() -> void:
	get_popup().add_theme_font_size_override("font_size", 8)
	get_popup().add_theme_font_override("font", fontstyle)
	get_popup().max_size = Vector2i()
	pass # Replace with function body.
