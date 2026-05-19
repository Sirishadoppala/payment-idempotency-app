const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || "http://localhost:7044"

async function request(url, options = {}) {
    const response = await fetch(`${API_BASE_URL}${url}`, options)
    let payload = null
    let text = null

    try {
        payload = await response.json()
    } catch (err) {
        text = await response.text().catch(() => null)
    }

    if (!response.ok) {
        console.error('Request failed:', {
            url: `${API_BASE_URL}${url}`,
            method: options.method,
            status: response.status,
            statusText: response.statusText,
            payload,
            text
        })
        const message = payload?.message || text || response.statusText || "Server request failed"
        const error = new Error(message)
        error.response = { status: response.status, data: payload ?? text }
        throw error
    }

    return payload
}

export async function createPayment(paymentData, idempotencyKey) {
    console.log('Sending payment request:', { paymentData, idempotencyKey })
    const data = await request("/api/Payment/Process", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Idempotency-Key": idempotencyKey,
        },
        body: JSON.stringify(paymentData),
    })
    console.log('Payment response:', data)
    return { data }
}

export async function getTransactions() {
    const data = await request("/api/Payment")
    return { data }
}

export async function getPaymentStatus(paymentId) {
    const data = await request(`/api/Payment/${paymentId}`)
    return { data }
}
