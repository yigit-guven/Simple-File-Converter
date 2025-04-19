from kivy.app import App
from kivy.uix.boxlayout import BoxLayout
from kivy.uix.button import Button
from kivy.uix.label import Label
from kivy.uix.spinner import Spinner
from kivy.uix.screenmanager import ScreenManager, Screen
from kivy.uix.widget import Widget
from kivy.core.window import Window
from kivy.uix.togglebutton import ToggleButton
from kivy.uix.filechooser import FileChooserIconView
from kivy.uix.popup import Popup
from kivy.uix.progressbar import ProgressBar
from kivy.clock import Clock
from kivy.properties import StringProperty, ObjectProperty, ListProperty
from kivy.metrics import dp

import os
from PIL import Image
from reportlab.pdfgen import canvas
from reportlab.lib.pagesizes import letter
from reportlab.platypus import SimpleDocTemplate, Paragraph
from reportlab.lib.styles import getSampleStyleSheet
import docx

# Set window size for desktop
Window.size = (dp(400), dp(700))
Window.minimum_width, Window.minimum_height = dp(400), dp(700)

class StyledButton(Button):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.font_size = '16sp'
        self.size_hint_y = None
        self.height = dp(50)
        self.bold = True
        self.background_normal = ''
        self.background_down = ''
        self.color = (1, 1, 1, 1)  # Ensure white text by default

class ConversionPopup(Popup):
    message = StringProperty("Converting...")
    progress = ObjectProperty(None)
    
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.title = "Conversion Status"
        self.size_hint = (0.8, 0.4)
        self.auto_dismiss = False
        
        layout = BoxLayout(orientation='vertical', padding=dp(20), spacing=dp(20))
        self.label = Label(text=self.message)
        self.progress = ProgressBar(max=100, value=0)
        
        layout.add_widget(self.label)
        layout.add_widget(self.progress)
        self.add_widget(layout)
    
    def update_progress(self, value, message=None):
        self.progress.value = value
        if message:
            self.message = message

class HomeScreen(Screen):
    file_path = StringProperty("")
    output_message = StringProperty("No file selected")
    conversion_options = [
        "DOCX to TXT",
        "JPG to PDF",
        "JPG to PNG",
        "PNG to JPG",
        "PNG to PDF",
        "TXT to PDF",
        "WEBP to PDF"
    ]
    
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.layout = BoxLayout(orientation='vertical', padding=dp(20), spacing=dp(15))
        
        # Header
        self.header = Label(
            text="Simple File Converter",
            font_size='24sp',
            bold=True,
            size_hint=(1, None),
            height=dp(60)
        )
        
        # File info
        self.file_label = Label(
            text=self.output_message,
            size_hint=(1, None),
            height=dp(60),
            font_size='16sp',
            halign="center",
            valign="middle",
            text_size=(Window.width - dp(40), None)
        )
        
        # Output location
        self.output_label = Label(
            text="Output will be saved in the same folder",
            size_hint=(1, None),
            height=dp(40),
            font_size='14sp',
            halign="center",
            valign="middle"
        )
        
        # Conversion type
        self.spinner = Spinner(
            text="Select conversion type",
            values=self.conversion_options,
            size_hint=(1, None),
            height=dp(50),
            font_size='16sp'
        )
        
        # Buttons
        self.pick_btn = StyledButton(text="Select File")
        self.convert_btn = StyledButton(text="Convert File")
        self.settings_btn = StyledButton(text="Settings")
        
        # Bindings
        self.pick_btn.bind(on_press=self.show_file_chooser)
        self.convert_btn.bind(on_press=self.convert_file)
        self.settings_btn.bind(on_press=self.goto_settings)
        
        # Add widgets
        self.layout.add_widget(self.header)
        self.layout.add_widget(self.file_label)
        self.layout.add_widget(self.output_label)
        self.layout.add_widget(self.spinner)
        self.layout.add_widget(self.pick_btn)
        self.layout.add_widget(self.convert_btn)
        self.layout.add_widget(self.settings_btn)
        
        self.add_widget(self.layout)
    
    def on_pre_enter(self):
        app = App.get_running_app()
        if app.output_dir:
            self.output_label.text = f"Output folder: {app.output_dir}"
        else:
            self.output_label.text = "Output will be saved in the same folder"
    
    def show_file_chooser(self, instance):
        content = BoxLayout(orientation='vertical', spacing=dp(10))
        file_chooser = FileChooserIconView()
        select_btn = StyledButton(text='Select')
        
        popup = Popup(title="Select a file", content=content, size_hint=(0.9, 0.9))
        
        def select_file(instance):
            if file_chooser.selection:
                self.file_path = file_chooser.selection[0]
                self.file_label.text = f"Selected: {os.path.basename(self.file_path)}"
            popup.dismiss()
        
        select_btn.bind(on_press=select_file)
        
        content.add_widget(file_chooser)
        content.add_widget(select_btn)
        popup.open()
    
    def convert_file(self, instance):
        app = App.get_running_app()
        
        if not self.file_path:
            self.file_label.text = "Please select a file first"
            return
        
        conversion = self.spinner.text
        save_dir = app.output_dir if app.output_dir != "None" else os.path.dirname(self.file_path)
        base = os.path.basename(self.file_path)
        name, ext = os.path.splitext(base)
        
        # Create and show progress popup
        popup = ConversionPopup()
        popup.open()
        
        # Simulate progress
        def update_progress(dt):
            if popup.progress.value < 20:
                popup.update_progress(20, "Starting conversion...")
            elif popup.progress.value < 50:
                popup.update_progress(50, "Processing file...")
            elif popup.progress.value < 80:
                popup.update_progress(80, "Finalizing...")
            else:
                popup.update_progress(100, "Conversion complete!")
                Clock.schedule_once(lambda dt: popup.dismiss(), 1)
                return False
            return True
        
        # Start conversion
        def start_conversion(dt):
            try:
                if conversion == "PNG to JPG" and ext.lower() == ".png":
                    img = Image.open(self.file_path).convert("RGB")
                    new_path = os.path.join(save_dir, f"{name}_converted.jpg")
                    img.save(new_path, "JPEG", quality=95)
                    self.file_label.text = f"Saved: {os.path.basename(new_path)}"
                
                elif conversion == "JPG to PNG" and ext.lower() in (".jpg", ".jpeg"):
                    img = Image.open(self.file_path)
                    new_path = os.path.join(save_dir, f"{name}_converted.png")
                    img.save(new_path, "PNG")
                    self.file_label.text = f"Saved: {os.path.basename(new_path)}"
                
                elif conversion == "TXT to PDF" and ext.lower() == ".txt":
                    new_path = os.path.join(save_dir, f"{name}_converted.pdf")
                    styles = getSampleStyleSheet()
                    doc = SimpleDocTemplate(new_path, pagesize=letter)
                    story = []
                    
                    with open(self.file_path, "r", encoding="utf-8") as f:
                        for line in f:
                            p = Paragraph(line, styles["Normal"])
                            story.append(p)
                    
                    doc.build(story)
                    self.file_label.text = f"Saved: {os.path.basename(new_path)}"
                
                elif conversion == "DOCX to TXT" and ext.lower() == ".docx":
                    new_path = os.path.join(save_dir, f"{name}_converted.txt")
                    doc = docx.Document(self.file_path)
                    with open(new_path, "w", encoding="utf-8") as f:
                        for para in doc.paragraphs:
                            f.write(para.text + "\n")
                    self.file_label.text = f"Saved: {os.path.basename(new_path)}"
                
                elif conversion == "JPG to PDF" and ext.lower() in (".jpg", ".jpeg"):
                    new_path = os.path.join(save_dir, f"{name}_converted.pdf")
                    img = Image.open(self.file_path)
                    img.save(new_path, "PDF", resolution=100.0)
                    self.file_label.text = f"Saved: {os.path.basename(new_path)}"
                
                elif conversion == "PNG to PDF" and ext.lower() == ".png":
                    new_path = os.path.join(save_dir, f"{name}_converted.pdf")
                    img = Image.open(self.file_path)
                    img.save(new_path, "PDF", resolution=100.0)
                    self.file_label.text = f"Saved: {os.path.basename(new_path)}"
                
                elif conversion == "WEBP to PDF" and ext.lower() == ".webp":
                    new_path = os.path.join(save_dir, f"{name}_converted.pdf")
                    img = Image.open(self.file_path)
                    img.save(new_path, "PDF", resolution=100.0)
                    self.file_label.text = f"Saved: {os.path.basename(new_path)}"
                
                else:
                    self.file_label.text = "Unsupported conversion or file type"
                    popup.dismiss()
                    return
                
                # Update progress to completion
                Clock.schedule_interval(update_progress, 0.1)
            
            except Exception as e:
                self.file_label.text = f"Error: {str(e)}"
                popup.dismiss()
        
        Clock.schedule_once(start_conversion, 0.5)
    
    def goto_settings(self, instance):
        self.manager.current = "settings"

class SettingsScreen(Screen):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.app = App.get_running_app()
        
        self.layout = BoxLayout(orientation='vertical', padding=dp(20), spacing=dp(15))
        
        # Header
        header = Label(
            text="Settings",
            font_size='24sp',
            bold=True,
            size_hint=(1, None),
            height=dp(60)
        )
        
        # Theme settings
        theme_box = BoxLayout(orientation='vertical', size_hint=(1, None), height=dp(100), spacing=dp(10))
        theme_label = Label(text="App Theme", font_size='18sp', size_hint=(1, None), height=dp(40))
        
        self.theme_toggle = ToggleButton(
            text="Dark Mode" if self.app.theme == "light" else "Light Mode",
            size_hint=(1, None),
            height=dp(50)
        )
        self.theme_toggle.bind(on_press=self.toggle_theme)
        
        theme_box.add_widget(theme_label)
        theme_box.add_widget(self.theme_toggle)
        
        # Add more spacing between theme and output sections
        self.layout.add_widget(header)
        self.layout.add_widget(theme_box)
        self.layout.add_widget(Widget(size_hint_y=None, height=dp(30)))  # Increased spacer height
        
        # Output directory
        output_box = BoxLayout(orientation='vertical', size_hint=(1, None), height=dp(150), spacing=dp(10))
        output_label = Label(text="Output Location", font_size='18sp', size_hint=(1, None), height=dp(-5))
        
        self.output_dir_label = Label(
            text=self.app.output_dir if self.app.output_dir != "None" else "Same as input file",
            font_size='14sp',
            halign="center",
            valign="middle",
            size_hint=(1, None),
            height=dp(60),
            text_size=(Window.width - dp(40), None)
        )
        
        choose_dir_btn = StyledButton(text="Change Output Folder")
        reset_dir_btn = StyledButton(text="Reset to Default")
        
        choose_dir_btn.bind(on_press=self.choose_directory)
        reset_dir_btn.bind(on_press=self.reset_directory)
        
        output_box.add_widget(output_label)
        output_box.add_widget(self.output_dir_label)
        output_box.add_widget(choose_dir_btn)
        output_box.add_widget(reset_dir_btn)
        
        # Credits
        credits = Label(
            text="Simple File Converter\nVersion 1.0\n\nDeveloped by Yigit Guven\n\n" +
                 "Licensed under CC BY-SA 4.0\n" +
                 "Source code: github.com/yigit-guven/Simple-File-Converter",
            font_size='14sp',
            halign="center",
            valign="bottom"
        )
        
        # Back button
        back_btn = StyledButton(text="Back to Home")
        back_btn.bind(on_press=lambda x: setattr(self.manager, 'current', 'home'))
        
        # Add widgets
        self.layout.add_widget(output_box)
        self.layout.add_widget(Widget(size_hint_y=1))  # Spacer
        self.layout.add_widget(credits)
        self.layout.add_widget(back_btn)
        
        self.add_widget(self.layout)
    
    def on_pre_enter(self):
        if self.app.output_dir and self.app.output_dir != "None":
            self.output_dir_label.text = self.app.output_dir
        else:
            self.output_dir_label.text = "Same as input file"
        
        self.theme_toggle.text = "Dark Mode" if self.app.theme == "light" else "Light Mode"
    
    def toggle_theme(self, instance):
        self.app.theme = "dark" if self.app.theme == "light" else "light"
        self.theme_toggle.text = "Dark Mode" if self.app.theme == "light" else "Light Mode"
        self.app.apply_theme()
    
    def choose_directory(self, instance):
        content = BoxLayout(orientation='vertical', spacing=dp(10))
        file_chooser = FileChooserIconView()
        
        # Create a properly styled select button
        select_btn = Button(
            text='Select This Folder',
            size_hint_y=None,
            height=dp(50),
            background_normal='',
            background_color=(0.2, 0.6, 0.86, 1),  # Blue background
            color=(1, 1, 1, 1),  # White text
            bold=True,
            font_size='16sp'
        )
        
        popup = Popup(title="Select output folder", content=content, size_hint=(0.9, 0.9))
        
        def select_folder(instance):
            if file_chooser.path:
                self.app.output_dir = file_chooser.path
                self.output_dir_label.text = self.app.output_dir
            popup.dismiss()
        
        select_btn.bind(on_press=select_folder)
        
        content.add_widget(file_chooser)
        content.add_widget(select_btn)
        popup.open()
    
    def reset_directory(self, instance):
        self.app.output_dir = "None"
        self.output_dir_label.text = "Same as input file"

class SimpleFileConverterApp(App):
    theme = StringProperty("light")
    output_dir = StringProperty("None")
    colors = ListProperty([0.95, 0.95, 0.95, 1])  # Default light background
    
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.theme = "light"
        self.output_dir = "None"
    
    def build(self):
        # Initialize theme before building screens
        self.apply_theme()
        
        sm = ScreenManager()
        sm.add_widget(HomeScreen(name="home"))
        sm.add_widget(SettingsScreen(name="settings"))
        
        # Apply theme again after screens are created
        Clock.schedule_once(lambda dt: self.apply_theme(), 0.1)
        return sm
    
    def apply_theme(self, *args):
        # Define colors for both themes
        if self.theme == "light":
            bg_color = [0.95, 0.95, 0.95, 1]  # Light gray background
            text_color = [0.1, 0.1, 0.1, 1]   # Dark text
            button_color = [0.2, 0.6, 0.86, 1]  # Blue buttons
            button_text = [1, 1, 1, 1]        # White button text
        else:
            bg_color = [0.1, 0.1, 0.1, 1]     # Dark background
            text_color = [0.9, 0.9, 0.9, 1]   # Light text
            button_color = [0.2, 0.5, 0.8, 1]  # Blue buttons
            button_text = [1, 1, 1, 1]        # White button text
        
        # Set background color for the app
        Window.clearcolor = bg_color
        
        # Update all widgets if root exists
        if hasattr(self, 'root') and self.root:
            for screen in self.root.screens:
                self.update_widget_colors(screen, bg_color, text_color, button_color, button_text)
    
    def update_widget_colors(self, widget, bg_color, text_color, button_color, button_text):
        if hasattr(widget, 'children'):
            for child in widget.children:
                self.update_widget_colors(child, bg_color, text_color, button_color, button_text)
        
        # Update specific widget properties
        if isinstance(widget, (Label, Spinner)):
            widget.color = text_color
        elif isinstance(widget, Button):
            widget.background_color = button_color
            widget.color = button_text
        elif isinstance(widget, ToggleButton):
            widget.background_color = button_color
            widget.color = button_text

if __name__ == "__main__":
    SimpleFileConverterApp().run()