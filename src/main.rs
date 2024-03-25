use hudhook::inject::Process;

fn main() -> Result<(), String> {
    Process::by_name("DJMAX RESPECT V.exe")
        .map_err(|e| format!("Failed to find target process: {:?}", e))?
        .inject("dp_payload.dll".into())
        .map_err(|e| format!("Failed to inject payload to target: {:?}", e))?;

    Ok(())
}
