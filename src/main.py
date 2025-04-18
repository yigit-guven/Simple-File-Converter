from kivy.app import App
from kivy.uix.label import Label

class FileConverterApp(App):
    def build(self):
        return Label(text = "Hello! This will be a file converter app.")
FileConverterApp().run()