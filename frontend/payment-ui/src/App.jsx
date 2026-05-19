import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import PaymentForm from './Components/PaymentForm'
import PaymentSuccess from './Pages/PaymentSuccess'
import PaymentFailed from './Pages/PaymentFailed'
import TransactionHistory from './Pages/TransactionHistory'
import './App.css'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<PaymentForm />} />
        <Route path="/success" element={<PaymentSuccess />} />
        <Route path="/failed" element={<PaymentFailed />} />
        <Route path="/history" element={<TransactionHistory />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App
