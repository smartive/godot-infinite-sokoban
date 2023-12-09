use gdnative::prelude::*;

fn init(handle: InitHandle) {
    handle.add_class::<LevelGenerator>();
}

godot_init!(init);

#[derive(NativeClass)]
#[inherit(Node)]
pub struct LevelGenerator;

#[methods]
impl LevelGenerator {
    fn new(_base: &Node) -> Self {
        LevelGenerator
    }

    #[method]
    fn generate_level(&self, height: i32, width: i32, box_count: i32) -> String {
        let level = sokoban_level_generator::generate_level(height as u8, width as u8, box_count as u8);
        sokoban_level_generator::encode_level(&level)
    }
}