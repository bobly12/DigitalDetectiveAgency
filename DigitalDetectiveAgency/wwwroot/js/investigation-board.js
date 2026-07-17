// State tracking object for selected elements
const deductionMatrix = {
    caseId: null,
    suspectId: null,
    evidenceId: null,
    witnessId: null
};

function selectItem(type, id, element) {
    // Remove active styling flag from sibling cards of same type
    const cards = document.querySelectorAll(`.${type}-card`);
    cards.forEach(c => c.classList.remove('selected-active'));

    // Wire selection to internal state dictionary
    element.classList.add('selected-active');
    deductionMatrix[`${type}Id`] = id;

    // Update the right-hand submission interface visually
    const slot = document.getElementById(`slot-${type}`);
    const itemTitle = element.querySelector('.card-title').innerText;
    slot.className = "filled-slot";
    slot.innerText = itemTitle;
}

async function submitDeduction(event) {
    event.preventDefault();

    deductionMatrix.caseId = parseInt(document.getElementById('caseIdInput').value);
    const feedbackBox = document.getElementById('terminal-feedback');
    feedbackBox.classList.remove('hidden', 'error-state');
    feedbackBox.innerText = "Processing clearance validation parameters...";

    // Guard statement asserting whole logical triangle is complete
    if (!deductionMatrix.suspectId || !deductionMatrix.evidenceId || !deductionMatrix.witnessId) {
        feedbackBox.classList.add('error-state');
        feedbackBox.innerText = "CRITICAL FAILURE: Hypothesis matrix components are incomplete.";
        return;
    }

    try {
        const response = await fetch('/api/investigation/accuse', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(deductionMatrix)
        });

        const result = await response.json();

        if (response.ok && result.success) {
            feedbackBox.innerText = `SUCCESS // WARR_AUTH_01 \nScore: +${result.score}\n\n${result.message}`;
            // Optional: Trigger game-over window or narrative unlock sequences here
        } else {
            feedbackBox.classList.add('error-state');
            feedbackBox.innerText = `REJECTED // ERR_LOGICAL_MISMATCH\n\n${result.message || "Arrest parameters invalid."}`;
        }
    } catch (error) {
        feedbackBox.classList.add('error-state');
        feedbackBox.innerText = "SYSTEM ERROR: Connection timeout during security clearance validation.";
    }
}