use hudhook::inject::Process;

fn main() {
    Process::by_name("DJMAX RESPECT V.exe").unwrap().inject("dp_payload.dll".into()).unwrap();
}
