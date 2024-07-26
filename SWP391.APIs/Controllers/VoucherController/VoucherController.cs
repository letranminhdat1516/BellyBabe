using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.Voucher;

[Route("api/[controller]")]
[ApiController]
public class VoucherController : ControllerBase
{
    private readonly VoucherService _voucherService;

    public VoucherController(VoucherService voucherService)
    {
        _voucherService = voucherService;
    }

    [HttpGet("GetAllVoucher")]
    public async Task<ActionResult<IEnumerable<Voucher>>> GetVouchers()
    {
        return await _voucherService.GetVouchersAsync();
    }

    [HttpGet("GetVoucherByID/{id}")]
    public async Task<ActionResult<Voucher>> GetVoucher(int id)
    {
        var voucher = await _voucherService.GetVoucherByIdAsync(id);

        if (voucher == null)
        {
            return NotFound();
        }

        return voucher;
    }

    [HttpPost("CreateVoucher")]
    public async Task<ActionResult<Voucher>> AddVoucher(VoucherDTO voucherDTO)
    {
        var voucher = await _voucherService.AddVoucherAsync(voucherDTO);
        return CreatedAtAction(nameof(GetVoucher), new { id = voucher.VoucherId }, voucher);
    }

    [HttpPut("UpdateVoucherByID/{id}")]
    public async Task<IActionResult> UpdateVoucher(int id, VoucherDTO voucherDTO)
    {
        var voucher = await _voucherService.UpdateVoucherAsync(id, voucherDTO);
        if (voucher == null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("DeleteVoucherById/{id}")]
    public async Task<IActionResult> DeleteVoucher(int id)
    {
        var result = await _voucherService.DeleteVoucherAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPost("validateCode")]
    public async Task<IActionResult> ValidateVoucher([FromQuery] string voucherCode)
    {
        var isValid = await _voucherService.ValidateVoucherWithQueueAsync(voucherCode);
        if (!isValid)
        {
            return BadRequest("Invalid or expired voucher code.");
        }
        return Ok("Voucher code is valid.");
    }
}
