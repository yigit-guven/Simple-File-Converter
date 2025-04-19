from kivy.app import App
from kivy.uix.boxlayout import BoxLayout
from kivy.uix.button import Button
from kivy.uix.label import Label
from kivy.uix.spinner import Spinner
import tkinter as tk
from tkinter import filedialog
import os
from PIL import Image

class FileConverterApp(App):
    def build(self):
        self.file_path = None

        layout = BoxLayout(orientation='vertical', padding=20, spacing=10)

        self.label = Label(text="No file selected")
        self.spinner = Spinner(
            text="Choose conversion",
            values=("PNG to JPG", "JPG to PNG"),
            size_hint=(1, None),
            height=44
        )
        pick_btn = Button(text="Pick a File", size_hint=(1, None), height=44)
        convert_btn = Button(text="Convert", size_hint=(1, None), height=44)

        pick_btn.bind(on_press=self.pick_file)
        convert_btn.bind(on_press=self.convert_file)

        layout.add_widget(self.label)
        layout.add_widget(self.spinner)
        layout.add_widget(pick_btn)
        layout.add_widget(convert_btn)

        return layout

    def pick_file(self, instance):
        root = tk.Tk()
        root.withdraw()
        file_path = filedialog.askopenfilename()

        if file_path:
            self.file_path = file_path
            self.label.text = f"Picked: {os.path.basename(file_path)}"
        else:
            self.label.text = "No file selected"

    def convert_file(self, instance):
        if not self.file_path:
            self.label.text = "Please pick a file first."
            return

        conversion = self.spinner.text
        if conversion == "PNG to JPG" and self.file_path.lower().endswith(".png"):
            img = Image.open(self.file_path).convert("RGB")
            new_path = self.file_path.replace(".png", "_converted.jpg")
            img.save(new_path, "JPEG")
            self.label.text = f"Saved: {os.path.basename(new_path)}"

        elif conversion == "JPG to PNG" and self.file_path.lower().endswith(".jpg"):
            img = Image.open(self.file_path)
            new_path = self.file_path.replace(".jpg", "_converted.png")
            img.save(new_path, "PNG")
            self.label.text = f"Saved: {os.path.basename(new_path)}"

        else:
            self.label.text = "Invalid conversion or file type."

FileConverterApp().run()