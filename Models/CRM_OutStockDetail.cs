using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_OutStockDetail
    {
        [JsonProperty(PropertyName = "crminvexportheaders")]
        public CRM_OutStockDetail_Header crminvexportheaders;
        public K3Cloud_OutStockBill ToOutStockBill()
        {
            var ins = new K3Cloud_OutStockBill();

            return ins;
        }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_OutStockDetail_Entry
    {
        [JsonProperty(PropertyName = "exOrderLinesId")]
        public int?  exOrderLinesId;//": 32854355,

        [JsonProperty(PropertyName = "inventoryId")]
        public int?  inventoryId;//"": 74876517,

        [JsonProperty(PropertyName = "inventoryCode")]
        public string?  inventoryCode;//"": null,

        [JsonProperty(PropertyName = "inventoryDesc")]
        public string?  inventoryDesc;//"": null,

        [JsonProperty(PropertyName = "locationFlag")]
        public string?  locationFlag;//"": null,

        [JsonProperty(PropertyName = "exOrderHeadersId")]
        public int?  exOrderHeadersId;//"": 12811734,

        [JsonProperty(PropertyName = "orderNo")]
        public string?  orderNo;//"": "YJRK202208230059",

        [JsonProperty(PropertyName = "materialCode")]
        public string?  materialCode;//"": "2600-R026.W",

        [JsonProperty(PropertyName = "unitCode")]
        public string?  unitCode;//"": "PCS",

        [JsonProperty(PropertyName = "quantity")]
        public int?  quantity;//"": 1,

        [JsonProperty(PropertyName = "barcode")]
        public string?  barcode;//"": null,

        [JsonProperty(PropertyName = "batchNo")]
        public string?  batchNo;//"": null,

        [JsonProperty(PropertyName = "versionNo")]
        public string?  versionNo;//"": null,

        [JsonProperty(PropertyName = "locationId")]
        public string?  locationId;//"": null,

        [JsonProperty(PropertyName = "locateInventoryCode")]
        public string?  locateInventoryCode;//"": null,

        [JsonProperty(PropertyName = "receiveInventoryId")]
        public int?  receiveInventoryId;//"": 70252904,

        [JsonProperty(PropertyName = "receiveInventoryCode")]
        public string?  receiveInventoryCode;//"": null,

        [JsonProperty(PropertyName = "receiveInventoryDesc")]
        public string?  receiveInventoryDesc;//"": null,

        [JsonProperty(PropertyName = "receiveLocationFlag")]
        public string?  receiveLocationFlag;//"": null,

        [JsonProperty(PropertyName = "receiveLocationId")]
        public string?  receiveLocationId;//"": null,

        [JsonProperty(PropertyName = "receiveLocateInventoryCode")]
        public string?  receiveLocateInventoryCode;//"": null,

        [JsonProperty(PropertyName = "counteractQuantity")]
        public string?  counteractQuantity;//"": null,

        [JsonProperty(PropertyName = "counteractLinesId")]
        public string?  counteractLinesId;//"": null,

        [JsonProperty(PropertyName = "barcodeUnitCode")]
        public string?  barcodeUnitCode;//"": null,

        [JsonProperty(PropertyName = "barcodeQuantity")]
        public string?  barcodeQuantity;//"": null,

        [JsonProperty(PropertyName = "originalType")]
        public string?  originalType;//"": "B1",

        [JsonProperty(PropertyName = "originalOrderId")]
        public int?  originalOrderId;//"": 31354296,

        [JsonProperty(PropertyName = "originalOrderNo")]
        public string?  originalOrderNo;//"": "YJOU202208230219",

        [JsonProperty(PropertyName = "originalLinesId")]
        public int?  originalLinesId;//"": 34421292,

        [JsonProperty(PropertyName = "originalDetailsId")]
        public int?  originalDetailsId;//"": 34421292,

        [JsonProperty(PropertyName = "sourceDetailsId")]
        public int?  sourceDetailsId;//"": 19467223,

        [JsonProperty(PropertyName = "sourceOrderId")]
        public int?  sourceOrderId;//"": 31354296,

        [JsonProperty(PropertyName = "sourceLinesId")]
        public int?  sourceLinesId;//"": 34421292,

        [JsonProperty(PropertyName = "sourceOrderNo")]
        public string?  sourceOrderNo;//"": "YJOU202208230219",

        [JsonProperty(PropertyName = "sourceType")]
        public string?  sourceType;//"": "B1",

        [JsonProperty(PropertyName = "barcodeEnabledFlag")]
        public string?  barcodeEnabledFlag;//"": "N",

        [JsonProperty(PropertyName = "versionEnabledFlag")]
        public string?  versionEnabledFlag;//"": "N",

        [JsonProperty(PropertyName = "batchEnabledFlag")]
        public string?  batchEnabledFlag;//"": "N",

        [JsonProperty(PropertyName = "counteractBarcodeQuantity")]
        public string?  counteractBarcodeQuantity;//"": null,

        [JsonProperty(PropertyName = "unitDesc")]
        public string?  unitDesc;//"": "PCS",

        [JsonProperty(PropertyName = "locateInventoryDesc")]
        public string?  locateInventoryDesc;//"": null,

        [JsonProperty(PropertyName = "receiveLocateInventoryDesc")]
        public string?  receiveLocateInventoryDesc;//"": null,

        [JsonProperty(PropertyName = "actualQuantity")]
        public int?  actualQuantity;//"": 1,

        [JsonProperty(PropertyName = "itemNo")]
        public int?  itemNo;//"": 1,

        [JsonProperty(PropertyName = "materialDesc")]
        public string?  materialDesc;//"": "电烤箱",

        [JsonProperty(PropertyName = "productModelCode")]
        public string?  productModelCode;//"": "2600-R026",

        [JsonProperty(PropertyName = "volumeUnit")]
        public string?  volumeUnit;//"": "M3",

        [JsonProperty(PropertyName = "volume")]
        public decimal?  volume;//"": 0.314547,

        [JsonProperty(PropertyName = "barnum")]
        public int?  barnum;//"": 0,

        [JsonProperty(PropertyName = "remark")]
        public string?  remark;//"": null,

        [JsonProperty(PropertyName = "interfaceLinesId")]
        public int?  interfaceLinesId;//"": 102497365,

        [JsonProperty(PropertyName = "customerRealQty")]
        public int?  customerRealQty;//"": 1,

        [JsonProperty(PropertyName = "receiveQuantity")]
        public string?  receiveQuantity;//"": null,

        [JsonProperty(PropertyName = "productTypeName")]
        public string?  productTypeName;//"": "电烤箱",

        [JsonProperty(PropertyName = "productTypeCode")]
        public string?  productTypeCode;//"": "I1",

        [JsonProperty(PropertyName = "subProductTypeName")]
        public string?  subProductTypeName;//"": null,

        [JsonProperty(PropertyName = "subProductTypeCode")]
        public string?  subProductTypeCode;//"": null,

        [JsonProperty(PropertyName = "twoProductTypeId")]
        public int?  twoProductTypeId;//"": 3429116,

        [JsonProperty(PropertyName = "twoProductTypeCode")]
        public string?  twoProductTypeCode;//"": "I1",

        [JsonProperty(PropertyName = "twoProductTypeName")]
        public string?  twoProductTypeName;//"": "电烤箱",

        [JsonProperty(PropertyName = "specification")]
        public string?  specification;//"": "2600-R026",

        [JsonProperty(PropertyName = "isPay")]
        public string?  isPay;//"": "否",

        [JsonProperty(PropertyName = "remark2")]
        public string?  remark2;//"": null,

        [JsonProperty(PropertyName = "deliveryGoodsStatus")]
        public string?  deliveryGoodsStatus;//"": "B",

        [JsonProperty(PropertyName = "receiveGoodsStatus")]
        public string?  receiveGoodsStatus;//"": "B",

        [JsonProperty(PropertyName = "deliveryGoodsStatusName")]
        public string?  deliveryGoodsStatusName;//"": "样品",

        [JsonProperty(PropertyName = "invQuantity")]
        public int?  invQuantity;//"": 0,

        [JsonProperty(PropertyName = "isSplit")]
        public string?  isSplit;//"": "N",

        [JsonProperty(PropertyName = "netPrice")]
        public string?  netPrice;//"": null,

        [JsonProperty(PropertyName = "approveAmount")]
        public string?  approveAmount;//"": null,

        [JsonProperty(PropertyName = "blastCapacity")]
        public string?  blastCapacity;//"": null,

        [JsonProperty(PropertyName = "allPressure")]
        public string?  allPressure;//"": null,

        [JsonProperty(PropertyName = "materialType")]
        public string?  materialType;//"": "1",

        [JsonProperty(PropertyName = "quality")]
        public int?  quality;//"": 45
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_OutStockDetail_Header
    {
        [JsonProperty(PropertyName = "exOrderHeadersId")]
        public int?  exOrderHeadersId;// 12811734,

        [JsonProperty(PropertyName = "orderNo")]
        public string?  orderNo;// "YJRK202208230059",

        [JsonProperty(PropertyName = "orderTypeId")]
        public int?  orderTypeId;// 906,

        [JsonProperty(PropertyName = "orderTypeName")]
        public string?  orderTypeName;// "样机入库申请（下样）",

        [JsonProperty(PropertyName = "orderTypeCode")]
        public string?  orderTypeCode;// "P_HAND_IN_YJ_CONFIRM",

        [JsonProperty(PropertyName = "orderSourceTypeCode")]
        public string?  orderSourceTypeCode;// "调拨单据",

        [JsonProperty(PropertyName = "orderDate")]
        public string?  orderDate;// "2022-08-23",

        [JsonProperty(PropertyName = "counteractHeadersId")]
        public string?  counteractHeadersId;// null,

        [JsonProperty(PropertyName = "reservationDeliveryDate")]
        public string?  reservationDeliveryDate;// "2022-08-23",

        [JsonProperty(PropertyName = "counteractHeadersOrderNo")]
        public string?  counteractHeadersOrderNo;// null,

        [JsonProperty(PropertyName = "counteractOrderTypeId")]
        public string?  counteractOrderTypeId;// null,

        [JsonProperty(PropertyName = "counteractOrderTypeName")]
        public string?  counteractOrderTypeName;// null,

        [JsonProperty(PropertyName = "status")]
        public string?  status;// "D1",

        [JsonProperty(PropertyName = "statusName")]
        public string?  statusName;// "已出货",

        [JsonProperty(PropertyName = "exportFlag")]
        public string?  exportFlag;// "N",

        [JsonProperty(PropertyName = "exportConfirmBy")]
        public string?  exportConfirmBy;// null,

        [JsonProperty(PropertyName = "exportConfirmDate")]
        public string?  exportConfirmDate;// null,

        [JsonProperty(PropertyName = "receiveFlag")]
        public string?  receiveFlag;// "N",

        [JsonProperty(PropertyName = "receiveBy")]
        public string?  receiveBy;// null,

        [JsonProperty(PropertyName = "receiveDate")]
        public string?  receiveDate;// null,

        [JsonProperty(PropertyName = "onwayFlag")]
        public string?  onwayFlag;// "N",

        [JsonProperty(PropertyName = "prebalanceFlag")]
        public string?  prebalanceFlag;// "N",

        [JsonProperty(PropertyName = "printDate")]
        public string?  printDate;// "2022-08-23",

        [JsonProperty(PropertyName = "printBy")]
        public string?  printBy;// null,

        [JsonProperty(PropertyName = "printCount")]
        public int?  printCount;// 1,

        [JsonProperty(PropertyName = "printLock")]
        public string?  printLock;// "N",

        [JsonProperty(PropertyName = "deptId")]
        public string?  deptId;// null,

        [JsonProperty(PropertyName = "deptName")]
        public string?  deptName;// null,

        [JsonProperty(PropertyName = "deptCode")]
        public string?  deptCode;// null,

        [JsonProperty(PropertyName = "orgId")]
        public int?  orgId;// 20,

        [JsonProperty(PropertyName = "orgName")]
        public string?  orgName;// "老板电器库存组织",

        [JsonProperty(PropertyName = "sourceOrderId")]
        public int?  sourceOrderId;// 31354296,

        [JsonProperty(PropertyName = "sourceType")]
        public string?  sourceType;// "B1",

        [JsonProperty(PropertyName = "sourceOrderNo")]
        public string?  sourceOrderNo;// "YJOU202208230219",

        [JsonProperty(PropertyName = "remark")]
        public string?  remark;// "永济申请撤r026回仓库\/KR02600017303467",

        [JsonProperty(PropertyName = "customerId")]
        public int?  customerId;// 20338713,

        [JsonProperty(PropertyName = "dmdId")]
        public int?  dmdId;// 40885084,

        [JsonProperty(PropertyName = "province")]
        public string?  province;// "130000",

        [JsonProperty(PropertyName = "city")]
        public string?  city;// "130900",

        [JsonProperty(PropertyName = "countyDistrict")]
        public string?  countyDistrict;// "130903",

        [JsonProperty(PropertyName = "street")]
        public string?  street;// null,

        [JsonProperty(PropertyName = "interfaceId")]
        public int?  interfaceId;// 40885084,

        [JsonProperty(PropertyName = "dmdNum")]
        public string?  dmdNum;// null,

        [JsonProperty(PropertyName = "customerCode")]
        public string?  customerCode;// "ORG-sjzb003",

        [JsonProperty(PropertyName = "customerName")]
        public string?  customerName;// "沧州市世纪厨具销售有限公司",

        [JsonProperty(PropertyName = "customerShortName")]
        public string?  customerShortName;// "石家庄",

        [JsonProperty(PropertyName = "vendorId")]
        public string?  vendorId;// null,

        [JsonProperty(PropertyName = "vendorCode")]
        public string?  vendorCode;// null,

        [JsonProperty(PropertyName = "vendorName")]
        public string?  vendorName;// null,

        [JsonProperty(PropertyName = "transEntityNo")]
        public string?  transEntityNo;// null,

        [JsonProperty(PropertyName = "customerContact")]
        public string?  customerContact;// null,

        [JsonProperty(PropertyName = "customerAddressId")]
        public string?  customerAddressId;// null,

        [JsonProperty(PropertyName = "address")]
        public string?  address;// "河北省沧州市运河区永济东路61号",

        [JsonProperty(PropertyName = "operatorName")]
        public string?  operatorName;// null,

        [JsonProperty(PropertyName = "estimatedLoadingCubicSum")]
        public string?  estimatedLoadingCubicSum;// null,

        [JsonProperty(PropertyName = "actualLoadingCubicSum")]
        public string?  actualLoadingCubicSum;// null,

        [JsonProperty(PropertyName = "exportConfirmByName")]
        public string?  exportConfirmByName;// null,

        [JsonProperty(PropertyName = "receiveByName")]
        public string?  receiveByName;// null,

        [JsonProperty(PropertyName = "printByName")]
        public string?  printByName;// null,

        [JsonProperty(PropertyName = "createdByName")]
        public string?  createdByName;// "李全双",\

        [JsonProperty(PropertyName = "lastUpdatedByName")]
        public string?  lastUpdatedByName;// "李全双",

        [JsonProperty(PropertyName = "creationDate")]
        public string?  creationDate;// "2022-08-23",

        [JsonProperty(PropertyName = "lastUpdateDate")]
        public string?  lastUpdateDate;// "2022-08-23",

        [JsonProperty(PropertyName = "createdBy")]
        public int?  createdBy;// 4963957,

        [JsonProperty(PropertyName = "lastUpdatedBy")]
        public int?  lastUpdatedBy;// 4963957,

        [JsonProperty(PropertyName = "inventoryId")]
        public int?  inventoryId;// 74876517,

        [JsonProperty(PropertyName = "isOutScan")]
        public string?  isOutScan;// "N",

        [JsonProperty(PropertyName = "receiveInventoryId")]
        public int?  receiveInventoryId;// 70252904,

        [JsonProperty(PropertyName = "isInScan")]
        public string?  isInScan;// "Y",

        [JsonProperty(PropertyName = "locationId")]
        public string?  locationId;// null,

        [JsonProperty(PropertyName = "receiveLocationId")]
        public string?  receiveLocationId;// null,

        [JsonProperty(PropertyName = "soOrderTypeId")]
        public string?  soOrderTypeId;// null,

        [JsonProperty(PropertyName = "productOrgId")]
        public string?  productOrgId;// null,

        [JsonProperty(PropertyName = "actualQuantityFlag")]
        public string?  actualQuantityFlag;// "N",

        [JsonProperty(PropertyName = "printFlag")]
        public string?  printFlag;// "Y",

        [JsonProperty(PropertyName = "productOrgName")]
        public string?  productOrgName;// null,

        [JsonProperty(PropertyName = "inventoryCode")]
        public string?  inventoryCode;// "1301030035",

        [JsonProperty(PropertyName = "inventoryDesc")]
        public string?  inventoryDesc;// "沧州永济路专卖店",

        [JsonProperty(PropertyName = "locationFlag")]
        public string?  locationFlag;// "N",

        [JsonProperty(PropertyName = "businessDistrictId")]
        public string?  businessDistrictId;// null,

        [JsonProperty(PropertyName = "receiveInventoryCode")]
        public string?  receiveInventoryCode;// "sjzb003001",

        [JsonProperty(PropertyName = "receiveInventoryDesc")]
        public string?  receiveInventoryDesc;// "沧州仓库",

        [JsonProperty(PropertyName = "receiveLocationFlag")]
        public string?  receiveLocationFlag;// "N",

        [JsonProperty(PropertyName = "receiveBusinessDistrictId")]
        public string?  receiveBusinessDistrictId;// null,

        [JsonProperty(PropertyName = "locationCode")]
        public string?  locationCode;// null,

        [JsonProperty(PropertyName = "locationDesc")]
        public string?  locationDesc;// null,

        [JsonProperty(PropertyName = "invLocationTypeCode")]
        public string?  invLocationTypeCode;// null,

        [JsonProperty(PropertyName = "receiveLocationCode")]
        public string?  receiveLocationCode;// null,

        [JsonProperty(PropertyName = "receiveLocationDesc")]
        public string?  receiveLocationDesc;// null,

        [JsonProperty(PropertyName = "receiveLocationTypeCode")]
        public string?  receiveLocationTypeCode;// null,

        [JsonProperty(PropertyName = "customerReceiveFlag")]
        public string?  customerReceiveFlag;// "N",

        [JsonProperty(PropertyName = "customerDate")]
        public string?  customerDate;// null,

        [JsonProperty(PropertyName = "receiptFlag")]
        public string?  receiptFlag;// "N",

        [JsonProperty(PropertyName = "receiptDate")]
        public string?  receiptDate;// null,

        [JsonProperty(PropertyName = "salesAreaId")]
        public string?  salesAreaId;// null,

        [JsonProperty(PropertyName = "salesAreaName")]
        public string?  salesAreaName;// null,

        [JsonProperty(PropertyName = "salesAreaId2")]
        public int?  salesAreaId2;// 66820,

        [JsonProperty(PropertyName = "customerTypeId")]
        public int?  customerTypeId;// 106,

        [JsonProperty(PropertyName = "isLock")]
        public string?  isLock;// null,

        [JsonProperty(PropertyName = "transType")]
        public string?  transType;// null,

        [JsonProperty(PropertyName = "isLockFlag")]
        public string?  isLockFlag;// "N",

        [JsonProperty(PropertyName = "remark1")]
        public string?  remark1;// "永济申请撤r026回仓库\/KR02600017303467",

        [JsonProperty(PropertyName = "customerAddress")]
        public string?  customerAddress;// null,

        [JsonProperty(PropertyName = "customerStoreId")]
        public string?  customerStoreId;// null,

        [JsonProperty(PropertyName = "counterInch")]
        public string?  counterInch;// null,

        [JsonProperty(PropertyName = "inceptAddress")]
        public string?  inceptAddress;// "河北省沧州市运河区河北省沧州市运河区永济东路61号",

        [JsonProperty(PropertyName = "contactTel")]
        public string?  contactTel;// "13012021878",

        [JsonProperty(PropertyName = "contactName")]
        public string?  contactName;// "赵敏",

        [JsonProperty(PropertyName = "fxOrderNo")]
        public string?  fxOrderNo;// "130103I04-2208230004",

        [JsonProperty(PropertyName = "deliveryCustomerId")]
        public int?  deliveryCustomerId;// 20338713,

        [JsonProperty(PropertyName = "deliveryCustomerName")]
        public string?  deliveryCustomerName;// "沧州市世纪厨具销售有限公司",

        [JsonProperty(PropertyName = "deliveryCustomerCode")]
        public string?  deliveryCustomerCode;// "ORG-sjzb003",

        [JsonProperty(PropertyName = "deliveryCustomerTypeId")]
        public int?  deliveryCustomerTypeId;// 106,

        [JsonProperty(PropertyName = "erpOrderNo")]
        public string?  erpOrderNo;// null,

        [JsonProperty(PropertyName = "provinceName")]
        public string?  provinceName;// "河北省",

        [JsonProperty(PropertyName = "cityName")]
        public string?  cityName;// "沧州市",

        [JsonProperty(PropertyName = "countyDistrictName")]
        public string?  countyDistrictName;// "运河区",

        [JsonProperty(PropertyName = "streetName")]
        public string?  streetName;// null,

        [JsonProperty(PropertyName = "sourceNo")]
        public string?  sourceNo;// "YJOU202208230219",

        [JsonProperty(PropertyName = "courierNumber")]
        public string?  courierNumber;// null,

        [JsonProperty(PropertyName = "courierCompany")]
        public string?  courierCompany;// null,

        [JsonProperty(PropertyName = "transportType")]
        public string?  transportType;// "B",

        [JsonProperty(PropertyName = "transportTypeName")]
        public string?  transportTypeName;// "自提",

        [JsonProperty(PropertyName = "inScanFlag")]
        public string?  inScanFlag;// "N",

        [JsonProperty(PropertyName = "outScanFlag")]
        public string?  outScanFlag;// null,

        [JsonProperty(PropertyName = "partorgood")]
        public string?  partorgood;// "GOOD",

        [JsonProperty(PropertyName = "isReceiveInv")]
        public string?  isReceiveInv;// null,

        [JsonProperty(PropertyName = "dealStatus")]
        public string?  dealStatus;// null,

        [JsonProperty(PropertyName = "dealInfo")]
        public string?  dealInfo;// null,

        [JsonProperty(PropertyName = "dealInStatus")]
        public string?  dealInStatus;// null,

        [JsonProperty(PropertyName = "dealInInfo")]
        public string?  dealInInfo;// null,

        [JsonProperty(PropertyName = "chauffeur")]
        public string?  chauffeur;// null,

        [JsonProperty(PropertyName = "vehicleBrand")]
        public string?  vehicleBrand;// null,

        [JsonProperty(PropertyName = "apptNumber")]
        public string?  apptNumber;// null,

        [JsonProperty(PropertyName = "storeName")]
        public string?  storeName;// null,

        [JsonProperty(PropertyName = "singlePerson")]
        public string?  singlePerson;// null,

        [JsonProperty(PropertyName = "singleDate")]
        public string?  singleDate;// null,

        [JsonProperty(PropertyName = "strategyContract")]
        public string?  strategyContract;// null,

        [JsonProperty(PropertyName = "engineeringProject")]
        public string?  engineeringProject;// null,

        [JsonProperty(PropertyName = "fxRemark")]
        public string?  fxRemark;// null,

        [JsonProperty(PropertyName = "contractNumber")]
        public string?  contractNumber;// null,

        [JsonProperty(PropertyName = "contractName")]
        public string?  contractName;// null,

        [JsonProperty(PropertyName = "kgName")]
        public string?  kgName;// null,

        [JsonProperty(PropertyName = "sumAmount")]
        public string?  sumAmount;// null,

        [JsonProperty(PropertyName = "exOrderNo")]
        public string?  exOrderNo;// null,

        [JsonProperty(PropertyName = "channelName")]
        public string?  channelName;// null,

        [JsonProperty(PropertyName = "channelId")]
        public string?  channelId;// null,

        [JsonProperty(PropertyName = "sdReturned")]
        public string?  sdReturned;// null,

        [JsonProperty(PropertyName = "sdInfo")]
        public string?  sdInfo;// null,

        [JsonProperty(PropertyName = "sdInReturned")]
        public string?  sdInReturned;// null,

        [JsonProperty(PropertyName = "sdInInfo")]
        public string?  sdInInfo;// null,

        [JsonProperty(PropertyName = "ifUpdateAct")]
        public string?  ifUpdateAct;// null,

        [JsonProperty(PropertyName = "ifUpdateCus")]
        public string?  ifUpdateCus;// null,

        [JsonProperty(PropertyName = "tel")]
        public string?  tel;// null,

        [JsonProperty(PropertyName = "fax")]
        public string?  fax;// null,

        [JsonProperty(PropertyName = "deliveryShortName")]
        public string?  deliveryShortName;// "石家庄",

        [JsonProperty(PropertyName = "deliveryTel")]
        public string?  deliveryTel;// null,

        [JsonProperty(PropertyName = "deliveryFax")]
        public string?  deliveryFax;// null,

        [JsonProperty(PropertyName = "lastUpdatedByName2")]
        public string?  lastUpdatedByName2;// null,

        [JsonProperty(PropertyName = "packagesQty")]
        public string?  packagesQty;// null,

        [JsonProperty(PropertyName = "exFlag")]
        public string?  exFlag;// null,

        [JsonProperty(PropertyName = "exByName")]
        public string?  exByName;// null,

        [JsonProperty(PropertyName = "exDate")]
        public string?  exDate;// null,

        [JsonProperty(PropertyName = "packageQty")]
        public string?  packageQty;// null,

        [JsonProperty(PropertyName = "timeSlot")]
        public string?  timeSlot;// null,

        [JsonProperty(PropertyName = "companyName")]
        public string?  companyName;// null,

        [JsonProperty(PropertyName = "destAddr")]
        public string?  destAddr;// null,

        [JsonProperty(PropertyName = "freightAddr")]
        public string?  freightAddr;// null,

        [JsonProperty(PropertyName = "crmInvExOrderLinesVs")]
        public List<CRM_OutStockDetail_Entry>? crmInvExOrderLinesVs;// null,
    }
}
