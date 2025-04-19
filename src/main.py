from kivy.app import App
from kivy.uix.boxlayout import BoxLayout
from kivy.uix.button import Button
from kivy.uix.label import Label
from kivy.uix.spinner import Spinner
from kivy.uix.widget import Widget
from kivy.core.window import Window

import tkinter as tk
from tkinter import filedialog
import os
from PIL import Image
from reportlab.pdfgen import canvas

# Set window size (useful when testing on desktop)
Window.size = (360, 640)

class FileConverterApp(App):
    def build(self):
        self.file_path = None

        root_layout = BoxLayout(orientation='vertical', padding=20, spacing=15)

        self.label = Label(
            text="No file selected",
            size_hint=(1, None),
            height=60,
            font_size=16,
            halign="center",
            valign="middle"
        )
        self.label.bind(size=self.label.setter('text_size'))

        self.spinner = Spinner(
            text="Choose conversion",
            values=("PNG to JPG", "JPG to PNG", "TXT to PDF"),
            size_hint=(1, None),
            height=50,
            background_color=(0.2, 0.5, 0.8, 1),
            color=(1, 1, 1, 1),
            font_size=16
        )

        pick_btn = Button(
            text="Pick a File",
            size_hint=(1, None),
            height=50,
            background_color=(0.2, 0.6, 0.4, 1),
            font_size=16
        )

        convert_btn = Button(
            text="Convert",
            size_hint=(1, None),
            height=50,
            background_color=(0.9, 0.4, 0.3, 1),
            font_size=16
        )

        pick_btn.bind(on_press=self.pick_file)
        convert_btn.bind(on_press=self.convert_file)

        root_layout.add_widget(Widget(size_hint_y=None, height=40))  # Top spacer
        root_layout.add_widget(self.label)
        root_layout.add_widget(self.spinner)
        root_layout.add_widget(pick_btn)
        root_layout.add_widget(convert_btn)
        root_layout.add_widget(Widget(size_hint_y=None, height=40))  # Bottom spacer

        return root_layout

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

        elif conversion == "TXT to PDF" and self.file_path.lower().endswith(".txt"):
            new_path = self.file_path.replace(".txt", "_converted.pdf")
            c = canvas.Canvas(new_path)
            with open(self.file_path, "r", encoding="utf-8") as f:
                lines = f.readlines()
                y = 800
                for line in lines:
                    c.drawString(72, y, line.strip())
                    y -= 15
            c.save()
            self.label.text = f"Saved: {os.path.basename(new_path)}"

        else:
            self.label.text = "Invalid conversion or file type."

FileConverterApp().run()