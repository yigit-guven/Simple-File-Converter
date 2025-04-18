import tkinter as tk
from tkinter import filedialog
import os

from PIL import Image

from kivy.app import App
from kivy.uix.boxlayout import BoxLayout
from kivy.uix.button import Button
from kivy.uix.label import Label

class FileConverterApp(App):
    def build(self):
        layout = BoxLayout(orientation = "vertical", padding = 20, spacing = 10)

        self.label = Label(text = "No file selected")
        btn = Button(text = "Pick a File")
        btn.bind(on_press = self.pick_file)

        layout.add_widget(self.label)
        layout.add_widget(btn)

        return layout
    def pick_file(self, instance):
        root = tk.Tk()
        root.withdraw()
        file_path = filedialog.askopenfilename(filetypes=[("PNG Images", "*.png")])

        if file_path:
            self.label.text = f"Picked: {os.path.basename(file_path)}"

            img = Image.open(file_path).convert("RGB")

            new_path = file_path.replace(".png", "_converted.jpg")
            img.save(new_path, "JPEG")

            self.label.text += f"\nSaved: {os.path.basename(new_path)}"
        else:
            self.label.text = 'No file selected'
FileConverterApp().run()