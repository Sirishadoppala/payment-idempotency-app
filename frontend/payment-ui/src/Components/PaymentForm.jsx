import { useState } from "react"
import { useNavigate, Link } from "react-router-dom"
import { createPayment } from "../api/PaymentApi"
import Loader from "./Loader"
import "./PaymentForm.css"

export default function PaymentForm() {
    const navigate = useNavigate()

    const [amount, setAmount] = useState("")
    const [receiver, setReceiver] = useState("")
    const [loading, setLoading] = useState(false)

    const handleSubmit = async (e) => {
        e.preventDefault()
        const paymentId = typeof crypto !== "undefined" && crypto.randomUUID
            ? crypto.randomUUID()
            : `txn-${Date.now()}-${Math.random().toString(36).slice(2, 10)}`
        const idempotencyKey = paymentId

        try {
            setLoading(true)
            const res = await createPayment({
                PaymentId: paymentId,
                UserId: receiver,
                Amount: Number(amount),
                Currency: "INR"
            }, idempotencyKey)

            setReceiver("")
            setAmount("")

            navigate("/success", {
                state: {
                    ...res.data,
                    paymentId
                }
            })
        } catch (error) {
            setReceiver("")
            setAmount("")
            navigate("/failed", {
                state: {
                    message: error.message,
                    status: error.response?.status,
                    details: error.response?.data
                }
            })
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="payment-wrapper">
            <div className="payment-card">
                <h2>Secure Payment</h2>
                <form onSubmit={handleSubmit}>
                    <label>Receiver Pay ID</label>
                    <input
                        type="text"
                        placeholder="eg. user@upi"
                        value={receiver}
                        onChange={(e) => setReceiver(e.target.value)}
                        required
                        className="receiver-input"
                    />
                    <label>Amount</label>
                    <input
                        type="number"
                        placeholder="Enter amount"
                        value={amount}
                        onChange={(e) => setAmount(e.target.value)}
                        required
                    />
                    <button disabled={loading}>
                        Pay Now
                    </button>
                </form>
                {loading && <Loader />}
                <Link to="/history" className="history-button">
                    View History
                </Link>
            </div>
        </div>
    )
}
