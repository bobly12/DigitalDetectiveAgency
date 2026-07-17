/**
 * Digital Detective Agency - Investigation Board Logic
 * Handles interactive desk selection and deduction form payload dispatch.
 */

// Global tracking object for the active hypothesis slots
window.investigationState = {
    suspectId: null,
    evidenceId: null,
    witnessId: null
};

/**
 * Handles the selection of evidence, witness transcripts, or suspect dossiers from the desk.
 * @param {string} type - The asset archetype ('suspect', 'evidence', 'witness')
 * @param {number} id - The database primary key identifier of the entity
 * @param {HTMLElement} cardElement - The specific DOM element that was interacted with
 */
window.selectItem = function (type, id, cardElement) {
    // 1. Manage active CSS highlight states on the workspace desk
    document.querySelectorAll(`.desk-card[data-type="${type}"]`).forEach(card => {
        card.classList.remove('selected-highlight');
    });
    cardElement.classList.add('selected-highlight');

    // 2. Extract the item title to update the UI terminal readouts
    const itemTitle = cardElement.querySelector('.card-title').textContent;

    // 3. Route target elements and update state properties
    const slotElement = document.getElementById(`slot-${type}`);
    if (!slotElement) return;

    // Update state state tracking metrics
    window.investigationState[`${type}Id`] = id;

    // Update slot layout to show selection instead of placeholder text
    slotElement.classList.remove('empty-slot');
    slotElement.classList.add('filled-slot');
    slotElement.textContent = itemTitle;
};

/**
 * Dispatches the accumulated hypothesis vectors to the API endpoint for validation.
 * @param {Event} event - The form submission trigger event object
 */
window.submitDeduction = function (event) {
    event.preventDefault();

    const feedbackTerminal = document.getElementById('terminal-feedback');
    const caseId = parseInt(document.getElementById('caseIdInput').value, 10);

    // Validate that a complete case hypothesis framework exists before tracking endpoint
    if (!window.investigationState.suspectId || !window.investigationState.evidenceId || !window.investigationState.witnessId) {
        showTerminalFeedback("❌ ERROR: MANDATE INCOMPLETE. You must select one Suspect, one Key Evidence piece, and one Contradictory Witness Transcript.", "error");
        return;
    }

    // Construct the data transmission object mirroring your backend endpoint expectations
    const payload = {
        caseId: caseId,
        accusedSuspectId: window.investigationState.suspectId,
        keyEvidenceId: window.investigationState.evidenceId,
        falseWitnessId: window.investigationState.witnessId
    };

    showTerminalFeedback("📡 TRANSMITTING WARRANT AUTHORIZATION TO CENTRAL MAINFRAME...", "info");

    // Execute the request dispatch to your controller endpoint
    fetch(`/Case/VerifyDeduction`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            // RequestVerificationToken handler for ASP.NET Core anti-forgery guardrails
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
        },
        body: JSON.stringify(payload)
    })
        .then(async response => {
            const data = await response.json();
            if (!response.ok) {
                throw new Error(data.message || "Mainframe response dropped. Network validation malfunction.");
            }
            return data;
        })
        .then(data => {
            if (data.isCorrect) {
                showTerminalFeedback(`🎉 CASE SOLVED: ${data.message || "Warrant authorized. The culprit is in custody. Excellent work, Detective."}`, "success");
                // Optional: Redirect or trigger post-game win state layouts here
            } else {
                showTerminalFeedback(`❌ ACCUSATION REJECTED: ${data.message || "Your hypothesis holds no water. The suspect's alibi remains solid."}`, "error");
            }
        })
        .catch(error => {
            showTerminalFeedback(`⚠️ SYSTEM ERROR: ${error.message}`, "error");
        });
};

/**
 * Renders status logs to the terminal box UI on the terminal dock component.
 * @param {string} message - The structural log payload string to output
 * @param {string} statusClass - Visual state variation ('info', 'success', 'error')
 */
function showTerminalFeedback(message, statusClass) {
    const feedbackTerminal = document.getElementById('terminal-feedback');
    if (!feedbackTerminal) return;

    feedbackTerminal.classList.remove('hidden', 'terminal-info', 'terminal-success', 'terminal-error');
    feedbackTerminal.classList.add(`terminal-${statusClass}`);
    feedbackTerminal.textContent = message;
}