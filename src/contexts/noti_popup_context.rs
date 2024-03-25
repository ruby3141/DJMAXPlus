use std::collections::VecDeque;

use imgui::Ui;

pub struct NotiPopupContext {
    notifications: VecDeque<Notification>,
    is_presenting: bool,
}

struct Notification {
    message: String,
}

impl NotiPopupContext {
    pub fn is_presenting(&self) -> bool { self.is_presenting }
}

impl NotiPopupContext {
    pub fn new() -> Self {
        Self {
            notifications: VecDeque::new(),
            is_presenting: false,
        }
    }

    pub fn push_notification(&mut self, message: String) {
        self.notifications.push_back(Notification {
            message: message,
        })
    }

    pub fn render(&mut self, ui: &mut Ui) {
        if !self.is_presenting && !self.notifications.is_empty() {
            self.is_presenting = true;
            ui.open_popup("notification");
        }
        ui.modal_popup("notification", || {
            ui.text(&self.notifications.front().unwrap().message);
            if ui.button("Ok") {
                self.is_presenting = false;
                self.notifications.pop_front();
                ui.close_current_popup();
            }
        });
    }
}
