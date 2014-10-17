function [ epos, bpos, IPOS, rsq, BETA] = estfundpos(Y, X1, X2, step, type, model, start, X0)
%============================================
%基金的股票债券仓位和行业配置测算工具
%============================================
%输入：各输入参数按日期顺序排列
%   Y:  基金净值的收益率序列
%   X1: 行业指数的收益率序列
%   X2: 债券指数的收益率序列
%   step:  时间序列长度
%   type: 基金类型；1=股票型，2=混合型
%   model: 模型类型
%   start: 当前运行位置
%输出：
%   epos: 股票仓位
%   bpos: 债券仓位
%   IPOS: 行业配置向量
%   rsq : 回归解释度
%模型类型：
%   1 = 指数回归法
%   2 = 指数主成份回归法
%   3 = 合成指数回归法
%=================================
    %不显示Warning
    warning('off');   
    
    BETA=[];
    switch model
        case 1
            [ epos, bpos, IPOS, rsq, BETA] = estfundposmodel1(Y, X1, X2, step, type, start, X0);
        case 2
            [ epos, bpos, IPOS, rsq] = estfundposmodel2(Y, X1, [], step, type, start);            
        case 3
            [ epos, bpos, IPOS, rsq] = estfundposmodel3(Y, X1, X2, step, type, start);
        otherwise
            throw(MException('Model:InputError','MODEL找不到该模型'));
    end

end

