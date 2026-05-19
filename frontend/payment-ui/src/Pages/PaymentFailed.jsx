import { useLocation, Link } from "react-router-dom"
import "./Pages.css"

export default function PaymentFailed(){
    const location = useLocation()
    const error = location.state

    return(
        <div className="status-page failed">
            <h2>Payment Failed</h2>
            <p>{error?.message || "Something went wrong while processing your payment."}</p>
            {error?.status && <p>Status Code: {error.status}</p>}
            {error?.details && (
                <div className="error-details">
                    <strong>Details:</strong>
                    <pre>{typeof error.details === 'string' ? error.details : JSON.stringify(error.details, null, 2)}</pre>
                </div>
            )}
            <div className="status-links">
                <Link to="/" className="home-button">← Back to Home</Link>
                <Link to="/history" className="home-button">View History</Link>
            </div>
        </div>
    )
}
